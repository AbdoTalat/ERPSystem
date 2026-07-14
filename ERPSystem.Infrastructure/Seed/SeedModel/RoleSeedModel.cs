using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Seed.SeedModel
{
    public class RoleSeedModel
    {
        public string Name { get; set; } = default!;
        public string NormalizedName { get; set; } = default!;
        public Guid TenantId { get; set; }
        public bool IsActive { get; set; }
    }

}
