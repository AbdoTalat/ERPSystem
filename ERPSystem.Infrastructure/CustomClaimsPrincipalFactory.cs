using ERPSystem.Application.DTOs.Branch;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HotelApp.Infrastructure
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly AppDbContext _context;

		public CustomClaimsPrincipalFactory(
			UserManager<AppUser> userManager,
			RoleManager<AppRole> roleManager,
			IOptions<IdentityOptions> optionsAccessor,
            AppDbContext context)
			: base(userManager, roleManager, optionsAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
		{
			var identity = await base.GenerateClaimsAsync(user);
            var roleIds = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync();
            var roles = await _context.Roles.IgnoreQueryFilters()
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            if (user.TenantId != Guid.Empty)
            {
                identity.AddClaim(new Claim("TenantId", user.TenantId.ToString()));
            }

            foreach (var roleName in roles)
            {
				var role = await _context.Roles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(r => r.Name == roleName);

				if (role == null) 
                    continue;

                var permissions = await _roleManager.GetClaimsAsync(role);

                identity.AddClaims(permissions.Where(c => c.Type == "Permission"));
            }


			var DefaultBranchId = _context.UserBranches
                .IgnoreQueryFilters()
				.Where(c => c.UserId == user.Id && c.IsDefault == true)
				.Select(b => b.BranchId)
				.FirstOrDefault();

            if (DefaultBranchId != 0)
            {
                identity.AddClaim(new Claim("DefaultBranchId", DefaultBranchId.ToString()));
            }

            // Add UserBranches Claims
            var userBranchesData = await _context.UserBranches
				.IgnoreQueryFilters()
				.Where(ub => ub.UserId == user.Id)
				.Select(ub => new UserBranchesDataDTO
				{
					Id = ub.BranchId,
					Name = ub.Branch.Name
				})
				.ToListAsync();

			if (userBranchesData.Any())
			{
				identity.AddClaim(new Claim("UserBranches", string.Join(",", userBranchesData.Select(b => b.Id))));
				identity.AddClaim(new Claim("UserBranchesData", JsonConvert.SerializeObject(userBranchesData)));
			}

			return identity;
		}
	}
    //public class CustomClaimsPrincipalFactory
    //: UserClaimsPrincipalFactory<AppUser, AppRole>
    //{
    //    private readonly UserManager<AppUser> _userManager;
    //    private readonly AppDbContext _context;

    //    public CustomClaimsPrincipalFactory(
    //        UserManager<AppUser> userManager,
    //        RoleManager<AppRole> roleManager,
    //        IOptions<IdentityOptions> optionsAccessor,
    //        AppDbContext context)
    //        : base(userManager, roleManager, optionsAccessor)
    //    {
    //        _userManager = userManager;
    //        _context = context;
    //    }

    //    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    //    {
    //        var identity = await base.GenerateClaimsAsync(user);

    //        // =========================
    //        // 1. BASIC USER CLAIMS
    //        // =========================
    //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

    //        if (user.TenantId != 0)
    //        {
    //            identity.AddClaim(new Claim("TenantId", user.TenantId.ToString()));
    //        }

    //        // =========================
    //        // 2. PERMISSIONS (OPTIMIZED)
    //        // =========================
    //        var roleClaims = await _userManager.GetRolesAsync(user);

    //        foreach (var roleName in roleClaims)
    //        {
    //            var permissions = await _roleManager.GetClaimsAsync(
    //                await _roleManager.FindByNameAsync(roleName)
    //            );

    //            identity.AddClaims(
    //                permissions.Where(c => c.Type == "Permission")
    //            );
    //        }

    //        // =========================
    //        // 3. USER BRANCHES (OPTIMIZED)
    //        // =========================
    //        var userBranches = await _context.UserBranches
    //            .Where(x => x.UserId == user.Id)
    //            .Select(x => new
    //            {
    //                x.BranchId,
    //                x.Branch.Name,
    //                x.IsDefault
    //            })
    //            .ToListAsync();

    //        var defaultBranch = userBranches.FirstOrDefault(x => x.IsDefault);

    //        if (defaultBranch != null)
    //        {
    //            identity.AddClaim(new Claim("DefaultBranchId", defaultBranch.BranchId.ToString()));
    //        }

    //        if (userBranches.Any())
    //        {
    //            identity.AddClaim(new Claim(
    //                "UserBranches",
    //                string.Join(",", userBranches.Select(x => x.BranchId))
    //            ));

    //            identity.AddClaim(new Claim(
    //                "UserBranchesData",
    //                JsonConvert.SerializeObject(userBranches.Select(x => new
    //                {
    //                    x.BranchId,
    //                    x.Name
    //                }))
    //            ));
    //        }

    //        return identity;
    //    }
    //}
}
