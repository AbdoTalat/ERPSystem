using ERPSystem.Application.DTOs.Invoice;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.InvoiceService
{
    public interface IInvoiceService
    {
        Task<ApiResponseHelper<IEnumerable<GetInvoiceDTO>>> GetAllInvoicesAsync();
        Task<ApiResponseHelper<GetInvoiceDTO>> GetInvoiceByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetInvoiceDTO>>> GetInvoicesBySalesOrderIdAsync(int SalesOrderId);
        Task<ApiResponseHelper<GetInvoiceDTO>> AddInvoiceAsync(int SalesOrderId);
        Task<ApiResponseHelper<GetInvoiceDTO>> CancellInvoiceByIdAsync(int Id);
        Task<ApiResponseHelper<object>> DeleteInvoiceAsync(int Id);
    }
}
