using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Domain.Common;

namespace ERPSystem.Domain.Entities
{
    public class Department : BaseEntity, IHasBranch, IHasTenant
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
