using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Domain.Common;

namespace ERPSystem.Domain.Entities
{
    public class Stock : BaseEntity, IHasBranch
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int DamagedQuantity { get; set; }
        public int AvailableQuantity { get; private set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; } = new HashSet<StockMovement>();
    }
}
