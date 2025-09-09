using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.API;
using ERPSystem.Application.DTOs.Department;

namespace ERPSystem.Application.Services.DepartmentService
{
    public interface IDepartmentService
    {
        public Task<ApiResponseHelper<IEnumerable<GetDepartmentDTO>>> GetAllDepartmentsAsync();
        public Task<ApiResponseHelper<GetDepartmentDTO>> GetDepartmentByIdAsync(int Id);
        public Task<ApiResponseHelper<GetDepartmentDTO>> AddDepartmentAsync(DepartmentDTO departmentDTO);
        public Task<ApiResponseHelper<GetDepartmentDTO>> EditDepartmentAsync(int Id, DepartmentDTO departmentDTO);
        public Task<ApiResponseHelper<string>> DeleteDepartmentAsync(int Id);
    }
}
