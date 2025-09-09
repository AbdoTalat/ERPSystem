using ERPSystem.Domain.Common;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class StockMovement : IHasBranch
    {
        public int Id { get; set; }

        public int StockId { get; set; }
        public Stock? Stock { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public int Quantity { get; set; } // Positive = In, Negative = Out
        public string? Reason { get; set; }

        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
    }
}
