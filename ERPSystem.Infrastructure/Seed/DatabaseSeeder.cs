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


        public DatabaseSeeder(AppDbContext context, IHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task SeedAsync()
        {
            await _context.SeedFromJsonAsync<Branch>(_env, "Branches.json");

            await SeedPermissionsAsync("Permissions.json");

        }

        private async Task SeedPermissionsAsync(string fileName)
        {
            var solutionRoot = Directory.GetParent(_env.ContentRootPath)?.FullName;

            var filePath = Path.Combine(
                solutionRoot!,
                "ERPSystem.Infrastructure",
                "Seed",
                "SeedData",
                fileName
            );

            if (!File.Exists(filePath))
                return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var permissionsDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);

            if (permissionsDict == null) return;

            // for role "Admin"
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
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
                        _context.RoleClaims.Add(new IdentityRoleClaim<int>
                        {
                            RoleId = adminRole.Id,
                            ClaimType = "Permission",
                            ClaimValue = permission
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }


    }
}
