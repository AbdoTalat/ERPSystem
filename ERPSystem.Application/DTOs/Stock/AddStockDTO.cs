using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Stock
{
    public class AddStockDTO
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int DamagedQuantity { get; set; }
    }
}
