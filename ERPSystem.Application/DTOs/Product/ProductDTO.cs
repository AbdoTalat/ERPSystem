using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Product
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int ReorderLevel { get; set; }
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}
