using AutoMapper;
using ERPSystem.Application.DTOs.Customer;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetCustomerDTO>>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Repository<Customer>().GetAllAsDtoAsync<GetCustomerDTO>();

            return ApiResponseHelper<IEnumerable<GetCustomerDTO>>.ResponseSuccess(data: customers);

        }
        public async Task<ApiResponseHelper<GetCustomerDTO>> GetCustomerByIdAsync(int Id)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsDtoAsync<GetCustomerDTO>(Id);

            if (customer == null)
            {
                return ApiResponseHelper<GetCustomerDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Customer Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetCustomerDTO>.ResponseSuccess(data: customer);
        }
        public async Task<ApiResponseHelper<GetCustomerDTO>> AddCustomerAsync(CustomerDTO dto)
        {
            try
            {
                var customerToAdd = _mapper.Map<Customer>(dto);
                await _unitOfWork.Repository<Customer>().AddNewAsync(customerToAdd);
                await _unitOfWork.CommitAsync();

                var addedCustomer = await _unitOfWork.Repository<Customer>().GetByIdAsDtoAsync<GetCustomerDTO>(customerToAdd.Id);

                return ApiResponseHelper<GetCustomerDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Customer Created Successfully", addedCustomer);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetCustomerDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.InnerException.Message);
            }
        }
        public async Task<ApiResponseHelper<GetCustomerDTO>> EditCustomerAsync(int Id, CustomerDTO dto)
        {
            var oldCustomer = await _unitOfWork.Repository<Customer>().GetByIdAsync(Id);
            if (oldCustomer == null)
            {
                return ApiResponseHelper<GetCustomerDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Customer Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(dto, oldCustomer);

                _unitOfWork.Repository<Customer>().Update(oldCustomer);
                await _unitOfWork.CommitAsync();

                var updatedCustomer = await _unitOfWork.Repository<Customer>().GetByIdAsDtoAsync<GetCustomerDTO>(Id);

                return ApiResponseHelper<GetCustomerDTO>.ResponseSuccess(message: "Customer Updated Successfully.", data: updatedCustomer);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetCustomerDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteCustomerAsync(int Id)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(Id);
            if (customer == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Customer Not found with ID: {Id}");
            }

            try
            {
                _unitOfWork.Repository<Customer>().Delete(customer);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message: $"{customer.Name} Customer Deleted Successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.CONFLICT, "Cannot delete customer because it is referenced by another record.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
