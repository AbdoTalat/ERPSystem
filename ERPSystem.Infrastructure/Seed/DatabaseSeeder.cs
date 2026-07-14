using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.DbContext;
using ERPSystem.Infrastructure.Seed.SeedModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Seed
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly IHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DatabaseSeeder(
            AppDbContext context,
            IHostEnvironment env,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // 1
            await _context.SeedFromJsonAsync<Tenant>( _env, "Tenants.json",
                tenant => _context.Tenants.IgnoreQueryFilters().AnyAsync(x => x.Id == tenant.Id));

            // 2
            await SeedRolesAsync("Roles.json");

            // 3
            await SeedUsersAsync("Users.json");

            // 4
            await _context.SeedFromJsonAsync<Branch>(_env, "Branches.json",
                branch => _context.Branches.IgnoreQueryFilters().AnyAsync(x => x.Id == branch.Id));

            // 5
            await SeedUserBranchesAsync("UserBranches.json"); 

            // 6
            //await _context.SeedFromJsonAsync<Category>(_env, "Categories.json",
            //    category => _context.Categories.AnyAsync(x => x.Id == category.Id));

            // 7
            //await _context.SeedFromJsonAsync<Product>(_env, "Products.json",
            //    product => _context.Products.AnyAsync(x => x.Id == product.Id));

            // 8
            await SeedPermissionsAsync("Permissions.json");
        }

        private async Task SeedRolesAsync(string fileName)
        {
            var roles = await SeedHelper.ReadJsonAsync<RoleSeedModel>(_env, fileName);

            foreach (var role in roles)
            {
                if (await _roleManager.Roles.IgnoreQueryFilters().AnyAsync(r => r.Name! == role.Name))
                    continue;

                await _roleManager.CreateAsync(new AppRole
                {
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    TenantId = role.TenantId,
                    IsActive = role.IsActive
                });
            }
        }
        private async Task SeedPermissionsAsync(string fileName)
        {
            var path = SeedHelper.GetSeedFilePath(_env, fileName);

            if (!File.Exists(path))
                return;

            var json = await File.ReadAllTextAsync(path);

            var permissions =
                JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            if (permissions == null)
                return;

            var adminRole = await _roleManager.Roles.IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
                return;

            var existingClaims = await _context.RoleClaims
                .Where(x => x.RoleId == adminRole.Id &&
                            x.ClaimType == "Permission")
                .Select(x => x.ClaimValue)
                .ToListAsync();

            foreach (var module in permissions)
            {
                foreach (var permission in module.Value)
                {
                    if (existingClaims.Contains(permission))
                        continue;

                    await _roleManager.AddClaimAsync(
                        adminRole,
                        new Claim("Permission", permission));
                }
            }
        }
        private async Task SeedUsersAsync(string fileName)
        {
            var users = await SeedHelper.ReadJsonAsync<UserSeedModel>(_env, fileName);

            foreach (var userSeed in users)
            {
                var user = await _userManager.Users.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserName == userSeed.UserName);

                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = userSeed.FirstName,
                        LastName = userSeed.LastName,
                        UserName = userSeed.UserName,
                        NormalizedUserName = userSeed.NormalizedUserName,
                        Email = userSeed.Email,
                        NormalizedEmail = userSeed.NormalizedEmail,
                        EmailConfirmed = userSeed.EmailConfirmed,
                        IsActive = userSeed.IsActive,
                        TenantId = userSeed.TenantId
                    };

                    var result = await _userManager.CreateAsync(user, userSeed.Password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Failed to create {userSeed.UserName}: " +
                            string.Join(", ",
                            result.Errors.Select(x => x.Description)));
                    }

                    foreach (var role in userSeed.Roles)
                    {
                        if (await _roleManager.Roles.IgnoreQueryFilters().AnyAsync(r => r.Name! == role))
                        {
                            //await _userManager.AddToRoleAsync(user, role);
                            await _context.UserRoles.AddAsync(new IdentityUserRole<int>
                            {
                                UserId = user.Id,
                                RoleId = (await _roleManager.Roles.IgnoreQueryFilters()
                                    .FirstOrDefaultAsync(r => r.Name == role))!.Id
                            });
                        }
                    }


                    await _context.SaveChangesAsync();
                }
            }
        }
        private async Task SeedUserBranchesAsync(string fileName)
        {
            var userBranches =
                await SeedHelper.ReadJsonAsync<UserBranchSeedModel>(_env, fileName);

            foreach (var item in userBranches)
            {
                var user = await _userManager.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.UserName == item.UserName);

                if (user == null)
                    throw new Exception($"User '{item.UserName}' was not found.");

                var branch = await _context.Branches
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(b => b.Name == item.BranchName);

                if (branch == null)
                    throw new Exception($"Branch '{item.BranchName}' was not found.");

                var exists = await _context.UserBranches
                    .IgnoreQueryFilters()
                    .AnyAsync(ub =>
                        ub.UserId == user.Id &&
                        ub.BranchId == branch.Id);

                if (exists)
                    continue;

                await _context.UserBranches.AddAsync(new UserBranches
                {
                    UserId = user.Id,
                    BranchId = branch.Id,
                    IsDefault = item.IsDefault,
                    TenantId = item.TenantId
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
