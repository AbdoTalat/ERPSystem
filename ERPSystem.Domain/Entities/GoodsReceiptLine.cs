using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class GoodsReceiptLine : IHasTenant
    {
        public int Id { get; set; }

        public int GoodsReceiptId { get; set; }
        public GoodsReceipt? GoodsReceipt { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int ReceivedQuantity { get; set; }
        public int DamagedQuantity { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
