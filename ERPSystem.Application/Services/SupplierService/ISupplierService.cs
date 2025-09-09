using ERPSystem.Application.DTOs.Supplier;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.SupplierService
{
    public interface ISupplierService
    {
        public Task<ApiResponseHelper<IEnumerable<GetSupplierDTO>>> GetAllSuppliersAsync();
        public Task<ApiResponseHelper<GetSupplierDTO>> GetSupplierByIdAsync(int Id);
        public Task<ApiResponseHelper<GetSupplierDTO>> AddSupplierAsync(SupplierDTO departmentDTO);
        public Task<ApiResponseHelper<GetSupplierDTO>> EditSupplierAsync(int Id, SupplierDTO departmentDTO);
        public Task<ApiResponseHelper<object>> DeleteSupplierAsync(int Id);
    }
}
