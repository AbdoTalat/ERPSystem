using ERPSystem.Domain.Common;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public sealed class Tenant 
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();
        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();
    }
}
