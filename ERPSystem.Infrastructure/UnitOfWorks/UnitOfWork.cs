using AutoMapper;
using ERPSystem.Application.Services.UserContext;
using ERPSystem.Domain;
using ERPSystem.Domain.Common;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.UnitOfWorks
{
    public class unitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        //private IDbContextTransaction? _transaction;
        private readonly IConfigurationProvider _mapperConfig;
        private readonly IUserContext _userContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public unitOfWork(AppDbContext context, 
            IConfigurationProvider mapperConfig, 
            IUserContext userContext)
        {
            _context = context;
            _mapperConfig = mapperConfig;
            _userContext = userContext;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>)_repositories[typeof(T)];

            var repo = new GenericRepository<T>(_context, _mapperConfig);
            _repositories[typeof(T)] = repo;
            return repo;
        }

        public async Task<int> CommitAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default)
        {
            if (!skipAuditFields)
            {
                ApplyAuditFields();
            }
            ApplyTenant();    
            return await _context.SaveChangesAsync(cancellationToken);
        }
        private void ApplyAuditFields()
        {
            var entries = _context.ChangeTracker
                .Entries<AuditableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            DateTime now = DateTime.UtcNow;
            var userId = _userContext.UserId;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                    entry.Entity.CreatedById = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdatedDate = now;
                    entry.Entity.LastUpdatedById = userId;
                }
            }
        }
        private void ApplyTenant()
        {
            foreach (var entry in _context.ChangeTracker
                .Entries<IHasTenant>()
                .Where(x => x.State == EntityState.Added))
            {
                var tenantId = _userContext.TenantId;
                if (tenantId.HasValue)
                    entry.Entity.TenantId = tenantId.Value;
                else
                    throw new InvalidOperationException("TenantId is not available in the user context.");
            }
        }

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
