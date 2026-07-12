using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class SalesOrderLine : IHasTenant
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
