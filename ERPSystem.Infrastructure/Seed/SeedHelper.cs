using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Seed
{
    public static class SeedHelper
    {
        public static async Task<List<T>> ReadJsonAsync<T>(IHostEnvironment env, string fileName)
        {
            var path = GetSeedFilePath(env, fileName);

            if (!File.Exists(path))
                return [];

            var json = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<List<T>>(json) ?? [];
        }

        public static string GetSeedFilePath(IHostEnvironment env, string fileName)
        {
            var solutionRoot = Directory.GetParent(env.ContentRootPath)!.FullName;

            return Path.Combine(
                solutionRoot,
                "ERPSystem.Infrastructure",
                "Seed",
                "SeedData",
                fileName);
        }
    }

}
