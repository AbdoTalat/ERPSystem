using ERPSystem.Domain.Common;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class StockMovement : BaseEntity, IHasBranch, IHasTenant
    {
        public int StockId { get; set; }
        public Stock? Stock { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public int Quantity { get; set; } // Positive = In, Negative = Out
        public string? Reason { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
