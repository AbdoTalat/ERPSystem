using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Supplier : BaseEntity, IHasBranch, IHasTenant
    {
        public string Name { get; set; } 
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
