using inmobiliaria.core.Common;
using inmobiliaria.core.Interfaces;
using inmobiliaria.infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.infrastructure.Service.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly RealEstateDbContext context;
        private readonly DbSet<TEntity> dbSet;
        public Repository(RealEstateDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            return await BuildQuery(filter, orderBy, includeProperties).AsNoTracking().ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetPagedListAsync(int pageNumber,
            int pageSize, Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties);

            PagedList<TEntity> pagedList = new()
            {
                PageSize = pageSize <= 0 ? 1 : pageSize,
                PageNumber = pageNumber <= 0 ? 1 : pageNumber,
                TotalPages = (int)Math.Ceiling((double)await query.AsNoTracking().CountAsync() / pageSize)
            };
            query = query.Skip((pagedList.PageNumber - 1) * pagedList.PageSize).Take(pagedList.PageSize);
            pagedList.List = await query.AsNoTracking().ToListAsync();
            return pagedList;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Added;
            await dbSet.AddAsync(entity);
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Add(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            dbSet.Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        public virtual Task AddRange(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                dbSet.AttachRange(entities);
                context.Entry(entities.First()).State = EntityState.Added;
            }

            return Task.CompletedTask;
        }
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity? entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity? entityToDelete)
        {
            if (entityToDelete != null)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            }
        }
        public virtual void DeleteRange(IEnumerable<TEntity> entityToDelete)
        {
            if (entityToDelete != null && entityToDelete.Any())
            {
                if (context.Entry(entityToDelete.First()).State == EntityState.Detached)
                {
                    dbSet.AttachRange(entityToDelete);
                }
                dbSet.RemoveRange(entityToDelete);
            }
        }
        public virtual void Update(TEntity entityToUpdate)
        {
            context.Entry(entityToUpdate).State = EntityState.Modified;
            dbSet.Update(entityToUpdate);
        }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public virtual async Task UpdateAsync(TEntity entityToUpdate)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        {
            context.Entry(entityToUpdate).State = EntityState.Modified;
            dbSet.Update(entityToUpdate);
        }
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter)
        {
            return filter == null ? await dbSet.CountAsync() : await dbSet.CountAsync(filter);
        }
        private IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

    }
}
