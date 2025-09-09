using ERPSystem.Application.IRepository;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Repositories
{
    public class PermissionLoader : IPermissionLoader
    {
        private readonly IHostEnvironment _env;

        public PermissionLoader(IHostEnvironment env)
        {
            _env = env;
        }

        public List<string> LoadAllPermissions()
        {
            var filePath = GetPermissionFilePath();
            var json = File.ReadAllText(filePath);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            return dict?.SelectMany(x => x.Value).Distinct().ToList() ?? new();
        }

        public Dictionary<string, List<string>> LoadGroupedPermissions()
        {
            var filePath = GetPermissionFilePath();
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json)
                   ?? new();
        }

        private string GetPermissionFilePath()
        {
            var solutionRoot = Directory.GetParent(_env.ContentRootPath)?.FullName;

            var filePath = Path.Combine(
                solutionRoot!,
                "ERPSystem.Infrastructure",
                "Seed",
                "SeedData",
                "Permissions.json"
            );

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Permission file not found.", filePath);
            return filePath;
        }
    }
}
