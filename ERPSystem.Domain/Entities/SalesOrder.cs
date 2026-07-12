using ERPSystem.Domain.Common;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class SalesOrder : BaseEntity, IHasBranch, IHasTenant
    {
        public DateTime OrderDate { get; set; }
        public SalesOrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
