using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.SalesOrder
{
    public class SalesOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<SalesOrderLineDTO> Lines { get; set; } = new List<SalesOrderLineDTO>();
    }
    public class SalesOrderLineDTO
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}
