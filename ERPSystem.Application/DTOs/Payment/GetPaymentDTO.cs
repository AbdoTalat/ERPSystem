using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Payment
{
    public class GetPaymentDTO
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }

        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceTotalAmount { get; set; }
        public string InvoiceStatus { get; set; }
    }
}
