using ERPSystem.Application.DTOs.GoodsReceipt;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.GoodsReceiptService
{
    public interface IGoodsReceiptService
    {
        Task<ApiResponseHelper<GetGoodsReceiptDTO>> ReceiveGoodsAsync(int purchaseOrderId, int warehouseId, ReceiveGoodsDTO dto, int userId);
    }
}
