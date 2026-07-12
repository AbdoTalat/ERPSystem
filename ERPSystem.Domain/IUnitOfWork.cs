using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<T> Repository<T>() where T : class;
		Task<int> CommitAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default);

		//Task BeginTransactionAsync();
		//Task CommitTransactionAsync();
		//Task RollbackTransactionAsync();

    }
}
