using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Seed.SeedModel
{
    public class UserBranchSeedModel
    {
        public string UserName { get; set; } = default!;
        public string BranchName { get; set; } = default!;
        public bool IsDefault { get; set; }
        public Guid TenantId { get; set; }
    }

}
