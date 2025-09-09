using ERPSystem.Application.DTOs.Customer;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.CustomerService
{
    public interface ICustomerService
    {
        public Task<ApiResponseHelper<IEnumerable<GetCustomerDTO>>> GetAllCustomersAsync();
        public Task<ApiResponseHelper<GetCustomerDTO>> GetCustomerByIdAsync(int Id);
        public Task<ApiResponseHelper<GetCustomerDTO>> AddCustomerAsync(CustomerDTO dto);
        public Task<ApiResponseHelper<GetCustomerDTO>> EditCustomerAsync(int Id, CustomerDTO dto);
        public Task<ApiResponseHelper<object>> DeleteCustomerAsync(int Id);
    }
}
