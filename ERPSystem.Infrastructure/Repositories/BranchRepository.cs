using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Domain;
using ERPSystem.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERPSystem.Application.IRepository;

namespace ERPSystem.Infrastructure.Repositories
{
	public class BranchRepository : IBranchRepository
	{
		private readonly AppDbContext _context;

		public BranchRepository(AppDbContext context)
        {
			_context = context;
		}
		public async Task<int> GetDefaultBranchIdByUserIdAsync(int userId)
		{
			var DefualtBranchId = await _context.UserBranches
				.Where(ub => ub.UserId == userId && ub.IsDefault)
				.Select(ub => ub.BranchId)
				.FirstOrDefaultAsync();

			return DefualtBranchId;
		}

	}
}
