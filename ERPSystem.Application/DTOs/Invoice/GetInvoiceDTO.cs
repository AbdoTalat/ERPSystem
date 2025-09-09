using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Invoice
{
    public class GetInvoiceDTO
    {
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string CustomerName { get; set; }
    }
}
