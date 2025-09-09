using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Payment
{
    public class PaymentDTO
    {
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public string Method { get; set; }
    }
}
