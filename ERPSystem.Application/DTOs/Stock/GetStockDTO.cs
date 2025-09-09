using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Stock
{
    public class GetStockDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int DamagedQuantity { get; set; }
        public DateTime LastStockUpdate { get; set; }
    }
}
