using AutoMapper;
using AutoMapper.QueryableExtensions;
using ERPSystem.Application.DTOs.AppUser;
using ERPSystem.Application.IRepository;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
		private readonly AppDbContext _context;
		private readonly UserManager<AppUser> _userManager;
        private readonly IConfigurationProvider _mapperConfig;

        public UserRepository(AppDbContext context, UserManager<AppUser> userManager, IConfigurationProvider mapperConfig)
        {
			_context = context;
			_userManager = userManager;
            _mapperConfig = mapperConfig;
        }
		public async Task<GetUserBranchesDTO> GetUserBranchesByUserIdAsync(int userId)
		{
			var userBranches = await _context.UserBranches
				.Where(ub => ub.UserId == userId)
				.Select(ub => new
				{
					ub.Branch.Name,
					ub.IsDefault
				})
				.ToListAsync();

			var branchList = userBranches
				.Select(ub => new BranchNames
				{
					BranchName = ub.Name
				})
				.ToList();

			var defaultBranch = userBranches.FirstOrDefault(x => x.IsDefault);

			var dto = new GetUserBranchesDTO
			{
				DefaultBranchName = defaultBranch?.Name ?? string.Empty,
				BranchNames = branchList
			};

			return dto;
		}

	}
}
