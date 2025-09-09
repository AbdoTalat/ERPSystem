using ERPSystem.Application.DTOs.Payment;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponseHelper<IEnumerable<GetPaymentDTO>>> GetAllPaymentsAsync();
        Task<ApiResponseHelper<GetPaymentDTO>> GetPaymentByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetPaymentDTO>>> GetPaymentsByInvoiceIdAsync(int InvoiceId);
        Task<ApiResponseHelper<GetPaymentDTO>> AddPaymentAsync(int InvoiceId, PaymentDTO dto);
        Task<ApiResponseHelper<GetPaymentDTO>> EditPaymentAsync(int paymentId, PaymentDTO dto);
        Task<ApiResponseHelper<GetPaymentDTO>> RefundPaymentAsync(int paymentId, decimal refundAmount);
    }
}
