using ERPSystem.Domain.Common;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class PurchaseOrder : BaseEntity, IHasBranch
    {
        public DateTime OrderDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
        public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
    }
}
