using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Invoice
{
    public class InvoiceDTO
    {
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int SalesOrderId { get; set; }
    }
}
