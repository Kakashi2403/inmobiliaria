using inmobiliaria.core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        Task<PagedList<TEntity>> GetPagedListAsync(
            int pageNumber,
            int pageSize, Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        Task InsertAsync(TEntity entity);
        void Insert(TEntity entity);
        void Add(TEntity entity);

        Task AddAsync(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(object id);
        void DeleteRange(IEnumerable<TEntity> entityToDelete);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        //Task<IEnumerable<TEntity>> ExecuteQuery<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> f);
    }
}
