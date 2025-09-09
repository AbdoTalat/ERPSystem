using AutoMapper;
using ERPSystem.Application.DTOs.AppRole;
using ERPSystem.Application.IRepository;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities.Auth;
using Helper.API;
using Helper.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper,
            RoleManager<AppRole> roleManager, IRoleRepository roleRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }
        public async Task<ApiResponseHelper<IEnumerable<GetAllRolesDTO>>> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.Repository<AppRole>().GetAllAsDtoAsync<GetAllRolesDTO>(SkipBranchFilter: true);

            return ApiResponseHelper<IEnumerable<GetAllRolesDTO>>.ResponseSuccess(data: roles);
        }

        public async Task<ApiResponseHelper<GetRoleByIdDTO>> GetRoleByIdAsync(int Id)
        {
            var role = await _unitOfWork.Repository<AppRole>().GetByIdAsDtoAsync<GetRoleByIdDTO>(Id);
            if (role == null)
            {
                return ApiResponseHelper<GetRoleByIdDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Role with ID:{Id} not found");
            }

            return ApiResponseHelper<GetRoleByIdDTO>.ResponseSuccess(data: role);
        }
        public async Task<ApiResponseHelper<List<GetPermissionDTO>>> GetAllPermissionsAsync()
        {
            var permissions = await _roleRepository.GetAllPermissionsAsync();

            return ApiResponseHelper<List<GetPermissionDTO>>.ResponseSuccess(data: permissions);
        }
        public async Task<ApiResponseHelper<GetRoleByIdDTO>> AddRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                var roleToAdd = _mapper.Map<AppRole>(roleDTO);
                var result = await _roleManager.CreateAsync(roleToAdd);
                if (result.Succeeded)
                {
                    var addedRole = await _unitOfWork.Repository<AppRole>().GetByIdAsDtoAsync<GetRoleByIdDTO>(roleToAdd.Id, SkipBranchFilter: true);
                    return ApiResponseHelper<GetRoleByIdDTO>.ResponseSuccess(data: addedRole);
                }
                return ApiResponseHelper<GetRoleByIdDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, result.Errors.ToString());
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetRoleByIdDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetRoleByIdDTO>> EditRoleAsync(int Id, RoleDTO roleDTO)
        {
            var oldRole = await _roleManager.FindByIdAsync(Id.ToString());
            if (oldRole == null)
            {
                return ApiResponseHelper<GetRoleByIdDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Role not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(roleDTO, oldRole);
                var result = await _roleManager.UpdateAsync(oldRole);

                var updatedRole = await _unitOfWork.Repository<AppRole>().GetByIdAsDtoAsync<GetRoleByIdDTO>(Id);

                return ApiResponseHelper<GetRoleByIdDTO>.ResponseSuccess(StatusCodes.OK, "role updated successfully.", updatedRole);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetRoleByIdDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<ApiResponseHelper<string>> DeleteRoleAsync(int Id)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"Role not found with ID: {Id}");
            }
            var isRoleAssignedToUsers = await _roleRepository.IsRoleAssignedToAnyUserAsync(Id);

            if (isRoleAssignedToUsers)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, "Cannot delete role because it is assigned to users.");
            }
            try
            {
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(r => r.Description));
                    return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, errorMessages);
                }
                return ApiResponseHelper<string>.ResponseSuccess(message: $"role {role.Name} deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
