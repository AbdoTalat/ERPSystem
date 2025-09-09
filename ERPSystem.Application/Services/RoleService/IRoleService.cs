using ERPSystem.Application.DTOs.AppRole;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.RoleService
{
    public interface IRoleService
    {
        Task<ApiResponseHelper<IEnumerable<GetAllRolesDTO>>> GetAllRolesAsync();
        Task<ApiResponseHelper<GetRoleByIdDTO>> GetRoleByIdAsync(int Id);
        Task<ApiResponseHelper<List<GetPermissionDTO>>> GetAllPermissionsAsync();
        Task<ApiResponseHelper<GetRoleByIdDTO>> AddRoleAsync(RoleDTO roleDTO);
        Task<ApiResponseHelper<GetRoleByIdDTO>> EditRoleAsync(int Id, RoleDTO roleDTO);
        Task<ApiResponseHelper<string>> DeleteRoleAsync(int Id);

    }
}
