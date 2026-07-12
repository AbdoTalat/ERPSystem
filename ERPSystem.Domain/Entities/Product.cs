using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Domain.Common;

namespace ERPSystem.Domain.Entities
{
    public class Product : BaseEntity, IHasBranch, IHasTenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; } // Stock keeping unit
        public decimal Price { get; set; }
        public int ReorderLevel { get; set; }
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new HashSet<Stock>();
        public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
        public ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
        public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; set; } = new HashSet<GoodsReceiptLine>();

    }
}
