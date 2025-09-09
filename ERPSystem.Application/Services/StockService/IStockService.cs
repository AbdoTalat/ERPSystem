using ERPSystem.Application.DTOs.Stock;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.StockService
{
    public interface IStockService
    {
        Task<ApiResponseHelper<IEnumerable<GetStockDTO>>> GetAllStocksAsync();
        Task<ApiResponseHelper<GetStockDTO>> GetStockByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetStockDTO>>> GetStockByProductAndWarehouseAsync(int warehouseId , int productId);
        Task<ApiResponseHelper<IEnumerable<GetStockByWarehouseDTO>>> GetStockByWarehouseAsync(int warehouseId);
        Task<ApiResponseHelper<GetStockDTO>> AddStockAsync(AddStockDTO dto);
        Task<ApiResponseHelper<GetStockDTO>> IncreaseStockByIdAsync(int userId, IncreaseStockDTO dto, bool IsCommit = true);
        Task<ApiResponseHelper<GetStockDTO>> DecreaseStockByIdAsync(int userId, DecreaseStockDTO dto, bool IsCommit = true);
        Task<ApiResponseHelper<object>> TransferStockAsync(int userId, TransferStockDTO dto);
        Task<ApiResponseHelper<object>> DeleteStockByIdAsync(int Id);
    }
}
