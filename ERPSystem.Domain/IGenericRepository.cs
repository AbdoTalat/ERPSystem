using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes);
		Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
		Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes);
		Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes);
        Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class;
        
        Task AddNewAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);

		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities);

		void Delete(T entity);
		void DeleteRange(IEnumerable<T> entities);

		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false);
    }
}
