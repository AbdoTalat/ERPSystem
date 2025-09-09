using AutoMapper;
using ERPSystem.Application.DTOs.Employee;
using ERPSystem.Application.IRepository;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using Helper.API;
using Helper.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetEmployeesDTO>>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAllAsDtoAsync<GetEmployeesDTO>();

            return ApiResponseHelper<IEnumerable<GetEmployeesDTO>>.ResponseSuccess(data: employees);
        }
        public async Task<ApiResponseHelper<GetEmployeesDTO>> GetEmployeeByIdAsync(int Id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsDtoAsync<GetEmployeesDTO>(Id);

            if (employee == null)
            {
                return ApiResponseHelper<GetEmployeesDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Employee not found with ID: {Id}");
            }

            return ApiResponseHelper<GetEmployeesDTO>.ResponseSuccess(data: employee);
        }
        public async Task<ApiResponseHelper<IEnumerable<GetEmployeesDTO>>> GetEmployeesByDepartmentIdAsync(int DepartmentId)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllAsDtoAsync<GetEmployeesDTO>(e => e.DepartmentId == DepartmentId);

            return ApiResponseHelper<IEnumerable<GetEmployeesDTO>>.ResponseSuccess(data: employees);
        }
        public async Task<ApiResponseHelper<GetEmployeesDTO>> AddEmployeeAsync(EmployeeDTO EmployeeDTO)
        {
            try
            {
                var employee = _mapper.Map<Employee>(EmployeeDTO);


                await _unitOfWork.Repository<Employee>().AddNewAsync(employee);
                await _unitOfWork.CommitAsync();



                var addedEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsDtoAsync<GetEmployeesDTO>(employee.Id);

                return ApiResponseHelper<GetEmployeesDTO>.ResponseSuccess(StatusCodes.CREATED, "Employee cretaed successfully.", addedEmployee);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetEmployeesDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetEmployeesDTO>> EditEmployeeAsync(EmployeeDTO EmployeeDTO, int Id)
        {
            var oldEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsync(Id);
            if (oldEmployee == null)
            {
                return ApiResponseHelper<GetEmployeesDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Employee not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(EmployeeDTO, oldEmployee);

                _unitOfWork.Repository<Employee>().Update(oldEmployee);
                await _unitOfWork.CommitAsync();

                var updatedEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsDtoAsync<GetEmployeesDTO>(Id);

                return ApiResponseHelper<GetEmployeesDTO>.ResponseSuccess(message: "Employee Updated successfully.", data: updatedEmployee);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetEmployeesDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<string>> DeleteEmployeeAsync(int Id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(Id);
            if (employee == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"Employee not found with ID: {Id}");
            }
            try
            {
                _unitOfWork.Repository<Employee>().Delete(employee);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<string>.ResponseSuccess(message: $"Employee '{employee.FullName}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
