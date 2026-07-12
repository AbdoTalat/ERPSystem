using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities.Auth
{
    public class UserBranches : IHasTenant
    {
        public int Id { get; set; }
        public bool IsDefault { get; set; }

        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
