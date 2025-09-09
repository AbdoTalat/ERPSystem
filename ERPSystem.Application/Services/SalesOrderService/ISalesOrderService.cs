using ERPSystem.Application.DTOs.PurchaseOrder;
using ERPSystem.Application.DTOs.SalesOrder;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.SalesOrderService
{
    public interface ISalesOrderService
    {
        Task<ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>> GetAllSalesOrderAsync();
        Task<ApiResponseHelper<GetSalesOrderDTO>> GetSalesOrderByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>> GetSalesOrdersByCustomerIdAsync(int CustomerId);
        Task<ApiResponseHelper<GetSalesOrderDTO>> AddSalesOrderAsync(SalesOrderDTO dto);
        Task<ApiResponseHelper<GetSalesOrderDTO>> CompleteSalesOrderAsync(int salesOrderId, int userId);
        Task<ApiResponseHelper<GetSalesOrderDTO>> EditSalesOrderByIdAsync(int Id, SalesOrderDTO dto);
        Task<ApiResponseHelper<GetSalesOrderDTO>> ConfirmSalesOrderByIdAsync(int Id);
        Task<ApiResponseHelper<GetSalesOrderDTO>> CancelSalesOrderByIdAsync(int Id);
    }
}
