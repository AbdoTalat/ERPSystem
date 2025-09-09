using ERPSystem.Infrastructure.DbContext;
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
    public static class DbSeederExtensions
    {
        public static async Task SeedFromJsonAsync<T>(
            this AppDbContext context,
            IHostEnvironment env,
            string fileName) where T : class
        {
            var solutionRoot = Directory.GetParent(env.ContentRootPath)?.FullName;

            var filePath = Path.Combine(
                solutionRoot!,
                "ERPSystem.Infrastructure",
                "Seed",
                "SeedData",
                fileName
            );

            if (!File.Exists(filePath))
                return;

            var dbSet = context.Set<T>();

            if (await dbSet.AnyAsync())
                return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var entities = JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();

            if (entities.Count == 0)
                return;

            await dbSet.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }
    }


}
