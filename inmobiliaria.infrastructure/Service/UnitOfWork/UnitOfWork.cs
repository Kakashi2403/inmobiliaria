using inmobiliaria.core.Interfaces;
using inmobiliaria.infrastructure.Persistencia;
using inmobiliaria.infrastructure.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.infrastructure.Service.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly RealEstateDbContext _context;
        private readonly Dictionary<string, object?> _repositories;

        public UnitOfWork(RealEstateDbContext context)
        {
            _context = context;
            _repositories = new();
        }
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            string name = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(name))
            {
                object? repository = Activator.CreateInstance(typeof(Repository<>).MakeGenericType(new Type[] { typeof(TEntity) }), new object[] { _context });
                _repositories.Add(name, repository);
            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return _repositories[name] as Repository<TEntity>;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public async void Save()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
