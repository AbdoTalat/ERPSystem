using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class GoodsReceipt : BaseEntity, IHasBranch, IHasTenant
    {
        public DateTime ReceivedDate { get; set; }
        public string? Notes { get; set; }
        
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public int ReceivedById { get; set; }
        public Employee? ReceivedBy { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; set; } = new HashSet<GoodsReceiptLine>();
    }
}
