using AutoMapper;
using ERPSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Application.IRepository;
using Helper.Constants;
using Helper.API;
using ERPSystem.Domain.Entities;
using ERPSystem.Application.DTOs.Department;

namespace ERPSystem.Application.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetDepartmentDTO>>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsDtoAsync<GetDepartmentDTO>();

            return ApiResponseHelper<IEnumerable<GetDepartmentDTO>>.ResponseSuccess(data: departments);

        }
        public async Task<ApiResponseHelper<GetDepartmentDTO>> GetDepartmentByIdAsync(int Id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsDtoAsync<GetDepartmentDTO>(Id);

            if (department == null)
            {
                return ApiResponseHelper<GetDepartmentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Department Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetDepartmentDTO>.ResponseSuccess(data: department);
        }
        public async Task<ApiResponseHelper<GetDepartmentDTO>> AddDepartmentAsync(DepartmentDTO departmentDTO)
        {
            try
            {
                var departmentToAdd = _mapper.Map<Department>(departmentDTO);
                await _unitOfWork.Repository<Department>().AddNewAsync(departmentToAdd);
                await _unitOfWork.CommitAsync();

                var addedDepartment = await _unitOfWork.Repository<Department>().GetByIdAsDtoAsync<GetDepartmentDTO>(departmentToAdd.Id);

                return ApiResponseHelper<GetDepartmentDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Department Created Successfully", addedDepartment);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetDepartmentDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetDepartmentDTO>> EditDepartmentAsync(int Id, DepartmentDTO model)
        {
            var oldDepartment = await _unitOfWork.Repository<Department>().GetByIdAsync(Id);
            if (oldDepartment == null)
            {
                return ApiResponseHelper<GetDepartmentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Department Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(model, oldDepartment);

                _unitOfWork.Repository<Department>().Update(oldDepartment);
                await _unitOfWork.CommitAsync();

                var updatedDepartment = await _unitOfWork.Repository<Department>().GetByIdAsDtoAsync<GetDepartmentDTO>(Id);

                return ApiResponseHelper<GetDepartmentDTO>.ResponseSuccess(message: "Department Updated Successfully.", data: updatedDepartment);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetDepartmentDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<string>> DeleteDepartmentAsync(int Id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(Id);
            if (department == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"Department Not found with ID: {Id}");
            }

            //bool employeesExists = await _unitOfWork.Repository<Employee>().ExistsAsync(e => e.DepartmentId == Id);
            //if (employeesExists)
            //{
            //    return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, "Cannot delete the department because it has related employees.");
            //}

            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<string>.ResponseSuccess(message: $"{department.Name} Department Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
