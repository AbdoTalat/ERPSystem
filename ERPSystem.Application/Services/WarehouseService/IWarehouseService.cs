using ERPSystem.Application.DTOs.Warehouse;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.WarehouseService
{
    public interface IWarehouseService
    {
        public Task<ApiResponseHelper<IEnumerable<GetWarehousesDTO>>> GetAllWarehousesAsync();
        public Task<ApiResponseHelper<GetWarehousesDTO>> GetWarehouseByIdAsync(int Id);
        public Task<ApiResponseHelper<GetWarehousesDTO>> AddWarehouseAsync(WarehouseDTO dto);
        public Task<ApiResponseHelper<GetWarehousesDTO>> EditWarehouseAsync(int Id, WarehouseDTO dto);
        public Task<ApiResponseHelper<string>> DeleteWarehouseAsync(int Id);
    }
}
