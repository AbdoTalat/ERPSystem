using AutoMapper;
using ERPSystem.Domain;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure
{
    public class unitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        //private IDbContextTransaction? _transaction;
        private readonly IConfigurationProvider _mapperConfig;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public unitOfWork(AppDbContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>)_repositories[typeof(T)];

            var repo = new GenericRepository<T>(_context, _mapperConfig);
            _repositories[typeof(T)] = repo;
            return repo;
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);
        public async Task<int> CommitAsync(bool skipAuditFields, CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(skipAuditFields, cancellationToken);
       
        //public async Task BeginTransactionAsync()
        //{
        //    if (_transaction == null)
        //    {
        //        _transaction = await _context.Database.BeginTransactionAsync();
        //    }
        //}
        //public async Task CommitTransactionAsync()
        //{
        //    if (_transaction == null)
        //        throw new InvalidOperationException("No active transaction to commit.");

        //    try
        //    {
        //        await CommitAsync();
        //        await _transaction.CommitAsync();
        //    }
        //    catch
        //    {
        //        await RollbackTransactionAsync();
        //        throw;
        //    }
        //    finally
        //    {
        //        if (_transaction != null)
        //        {
        //            await _transaction.DisposeAsync();
        //            _transaction = null;
        //        }
        //    }
        //}
        //public async Task RollbackTransactionAsync()
        //{
        //    if (_transaction != null)
        //    {
        //        await _transaction.RollbackAsync();
        //        await _transaction.DisposeAsync();
        //        _transaction = null;
        //    }
        //}
        public void Dispose()
        {
           // _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
