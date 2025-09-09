using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.PurchaseOrder
{
    public class PurchaseOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }
        public List<PurchaseOrderLineDto> Lines { get; set; } = new List<PurchaseOrderLineDto>();
    }

    public class PurchaseOrderLineDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
