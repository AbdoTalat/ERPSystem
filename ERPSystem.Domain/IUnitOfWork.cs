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

		Task<int> CommitAsync(CancellationToken cancellationToken = default);
		Task<int> CommitAsync(bool skipAuditFields, CancellationToken cancellationToken = default);

	}
}
