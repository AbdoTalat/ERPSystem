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
    /// A user belongs to one tenant (company) and can have access to multiple branches within that tenant.
    /// Branch assignments are managed through the UserBranch entity.
    /// </summary>
    public class AppUser : IdentityUser<int>, IHasTenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }
        
        public int? LastUpdatedById { get; set; }
        public AppUser? LastUpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<UserBranches> UserBranches { get; set; } = new HashSet<UserBranches>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
