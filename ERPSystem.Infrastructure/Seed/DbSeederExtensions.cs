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
            string fileName,
            Func<T, Task<bool>> exists)
            where T : class
        {
            var entities = await SeedHelper.ReadJsonAsync<T>(env, fileName);

            foreach (var entity in entities)
            {
                if (!await exists(entity))
                {
                    context.Set<T>().Add(entity);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
