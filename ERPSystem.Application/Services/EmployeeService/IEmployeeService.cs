using ERPSystem.Application.DTOs.Employee;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ApiResponseHelper<IEnumerable<GetEmployeesDTO>>> GetAllEmployeesAsync();
        Task<ApiResponseHelper<GetEmployeesDTO>> GetEmployeeByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetEmployeesDTO>>> GetEmployeesByDepartmentIdAsync(int DepartmentId);
        Task<ApiResponseHelper<GetEmployeesDTO>> AddEmployeeAsync(EmployeeDTO EmployeeDTO);
        Task<ApiResponseHelper<GetEmployeesDTO>> EditEmployeeAsync(EmployeeDTO EmployeeDTO, int Id);
        Task<ApiResponseHelper<string>> DeleteEmployeeAsync(int Id);
    }
}
