using ERPSystem.Domain.Common;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Invoice : BaseEntity, IHasBranch
    {
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }

        public int SalesOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
