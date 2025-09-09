using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities.Auth
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public int? DefaultBranchId { get; set; }
        public Branch? DefaultBranch { get; set; }

        public int? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }
        
        public int? LastUpdatedById { get; set; }
        public AppUser? LastUpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public ICollection<UserBranches> UserBranches { get; set; } = new HashSet<UserBranches>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
