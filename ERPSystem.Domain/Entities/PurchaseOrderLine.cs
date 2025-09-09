using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class PurchaseOrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }
        public int PurchaseOrderId { get; set; }
  

        public Product? Product { get; set; }
        public int ProductId { get; set; }
    }
}
