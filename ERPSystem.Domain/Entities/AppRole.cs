using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities.Auth
{
    public class AppRole : IdentityRole<int>
    {
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedById { get; set; }
        public AppUser? LastUpdatedBy { get; set; }
    }
}
