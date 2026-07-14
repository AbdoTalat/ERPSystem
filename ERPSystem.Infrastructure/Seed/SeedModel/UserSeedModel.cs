using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Seed.SeedModel
{
    public class UserSeedModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string NormalizedUserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NormalizedEmail { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = true;
        public string Password { get; set; } = default!;
        public Guid TenantId { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = [];
    }

}
