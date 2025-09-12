using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.DbContext;
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
            await _context.SeedFromJsonAsync<Branch>(_env, "Branches.json");

            await SeedRolesAsync("Roles.json");
            await SeedUsersAsync("Users.json");
            await SeedPermissionsAsync("Permissions.json");
        }
        private async Task SeedPermissionsAsync(string fileName)
        {
            var solutionRoot = Directory.GetParent(_env.ContentRootPath)?.FullName;
            var filePath = Path.Combine(solutionRoot!, "ERPSystem.Infrastructure", "Seed", "SeedData", fileName);

            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var permissionsDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);

            if (permissionsDict == null) return;

            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null) return;

            var existingClaims = await _context.RoleClaims
                .Where(rc => rc.RoleId == adminRole.Id && rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue)
                .ToListAsync();

            foreach (var kvp in permissionsDict)
            {
                foreach (var permission in kvp.Value)
                {
                    if (!existingClaims.Contains(permission))
                    {
                        await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                    }
                }
            }
        }
        private async Task SeedRolesAsync(string fileName)
        {
            var solutionRoot = Directory.GetParent(_env.ContentRootPath)?.FullName;
            var filePath = Path.Combine(solutionRoot!, "ERPSystem.Infrastructure", "Seed", "SeedData", fileName);

            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var roles = JsonConvert.DeserializeObject<List<AppRole>>(jsonData);

            if (roles == null) return;

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new AppRole
                    {
                        Name = role.Name,
                        NormalizedName = role.NormalizedName
                    });
                }
            }
        }
        private async Task SeedUsersAsync(string fileName)
        {
            var solutionRoot = Directory.GetParent(_env.ContentRootPath)?.FullName;
            var filePath = Path.Combine(solutionRoot!, "ERPSystem.Infrastructure", "Seed", "SeedData", fileName);

            if (!File.Exists(filePath))
                return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var users = JsonConvert.DeserializeObject<List<UserSeedModel>>(jsonData);

            if (users == null) 
                return;

            foreach (var userSeed in users)
            {
                var user = await _userManager.FindByNameAsync(userSeed.UserName);

                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = userSeed.FirstName,
                        LastName = userSeed.LastName,
                        UserName = userSeed.UserName,
                        Email = userSeed.Email,
                        EmailConfirmed = userSeed.EmailConfirmed,
                        IsActive = userSeed.IsActive
                    };

                    var result = await _userManager.CreateAsync(user, userSeed.Password);

                    if (result.Succeeded)
                    {
                        foreach (var roleName in userSeed.Roles)
                        {
                            if (await _roleManager.RoleExistsAsync(roleName))
                            {
                                await _userManager.AddToRoleAsync(user, roleName);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception($"Failed to create user {userSeed.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                var allBranches = await _context.Branches.ToListAsync();
                foreach (var branch in allBranches)
                {
                    var exists = await _context.UserBranches
                        .AnyAsync(ub => ub.UserId == user.Id && ub.BranchId == branch.Id);

                    if (!exists)
                    {
                        await _context.UserBranches.AddAsync(new UserBranches
                        {
                            UserId = user.Id,
                            BranchId = branch.Id
                        });
                    }
                }

                if (user.DefaultBranchId == null && allBranches.Any())
                {
                    user.DefaultBranchId = allBranches.First().Id;
                    _context.Users.Update(user);
                }

                await _context.SaveChangesAsync();
            }

        }

        public class UserSeedModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public List<string> Roles { get; set; }
        }
    }
}
