using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.DTOs.PurchaseOrder;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.PurchaseOrderService
{
    public interface IPurchaseOrderService
    {        
        Task<ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>> GetAllPurchaseOrderAsync();
        Task<ApiResponseHelper<GetPurchaseOrderDTO>> GetPurchaseOrderByIdAsync(int Id);       
        Task<ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>> GetPurchaseOrdersBySupplierIdAsync(int SupplierId);
        Task<ApiResponseHelper<GetPurchaseOrderDTO>> AddPurchaseOrderAsync(PurchaseOrderDTO dto);       
        Task<ApiResponseHelper<GetPurchaseOrderDTO>> EditPurchaseOrderByIdAsync(int Id, PurchaseOrderDTO dto);
        Task<ApiResponseHelper<GetPurchaseOrderDTO>> ApprovePurchaseOrderByIdAsync(int Id);
        Task<ApiResponseHelper<GetPurchaseOrderDTO>> CancelPurchaseOrderByIdAsync(int Id);

    }
}
