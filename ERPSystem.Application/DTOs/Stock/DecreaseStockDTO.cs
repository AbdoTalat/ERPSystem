using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Stock
{
    public class DecreaseStockDTO
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }    // StockMovement
        public string? Reason { get; set; }  // StockMovement
    }
}
