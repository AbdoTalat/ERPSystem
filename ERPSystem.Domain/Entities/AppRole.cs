using ERPSystem.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities.Auth
{
    /// <summary>
    /// A role belongs to a single tenant (company) and defines the permissions assigned to users within that tenant.
    /// </summary>
    public class AppRole : IdentityRole<int>, IHasTenant
    {
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedById { get; set; }
        public AppUser? LastUpdatedBy { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
