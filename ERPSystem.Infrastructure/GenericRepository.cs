using AutoMapper.QueryableExtensions;
using ERPSystem.Domain;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Helper.Extentions;
using Helper.Context;

namespace ERPSystem.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		private readonly IConfigurationProvider _mapperConfig;
		private readonly DbSet<T> _dbSet;
		public GenericRepository(AppDbContext context,
			IConfigurationProvider mapperConfig)
		{
			_context = context;
			_mapperConfig = mapperConfig;
			_dbSet = _context.Set<T>();
		}

		#region Get Methods
		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);

			var a = query;

			if (predicate != null)
				query = query.Where(predicate);

			return await query.ToListAsync();
		}
		public async Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);


			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query.ProjectTo<TDto>(_mapperConfig).ToListAsync();
		}
		public async Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.Where(e => EF.Property<int>(e, "Id") == id)
				.BranchFilter(SkipBranchFilter);

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query
				.ProjectTo<TDto>(_mapperConfig)
				.FirstOrDefaultAsync();
		}
		public async Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.Where(e => EF.Property<int>(e, "Id") == id)
				.BranchFilter(SkipBranchFilter);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
			{
				query = query.Where(predicate);
			}
            return await query.FirstOrDefaultAsync();
		}
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet
                .AsNoTracking()
                .BranchFilter(SkipBranchFilter);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query
                .ProjectTo<TDto>(_mapperConfig)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Add & Edit & Delete
        public async Task AddNewAsync(T entity)
		{
			var branchId = BranchContext.CurrentBranchId;

			var prop = typeof(T).GetProperty("BranchId");
			if (prop != null && prop.PropertyType == typeof(int) && branchId.HasValue)
			{
				prop.SetValue(entity, branchId.Value);
			}

			await _dbSet.AddAsync(entity);
		}
		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			var branchId = BranchContext.CurrentBranchId;

			foreach (var entity in entities)
			{
				var prop = typeof(T).GetProperty("BranchId");
				if (prop != null && prop.PropertyType == typeof(int) && branchId.HasValue)
				{
					prop.SetValue(entity, branchId.Value);
				}
			}

			await _dbSet.AddRangeAsync(entities);
		}

		public void Update(T entity)
			=> _dbSet.Update(entity);
		public void UpdateRange(IEnumerable<T> entities)
			=> _dbSet.UpdateRange(entities);
		public void Delete(T entity)
		{
			var branchId = BranchContext.CurrentBranchId;
			var prop = typeof(T).GetProperty("BranchId");

			if (prop != null && prop.PropertyType == typeof(int) && branchId.HasValue)
			{
				var entityBranchId = (int?)prop.GetValue(entity);
				if (entityBranchId != branchId)
					throw new UnauthorizedAccessException("Cannot delete entity from another branch.");
			}

			_dbSet.Remove(entity);
		}
		public void DeleteRange(IEnumerable<T> entities)
			=> _dbSet.RemoveRange(entities);
		#endregion

		#region Other Methods
		public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false)
		{
			return await _dbSet
				.AsNoTracking()
				.BranchFilter(skipBranchFilter)
				.AnyAsync(predicate);
		}
		#endregion

	}
}
