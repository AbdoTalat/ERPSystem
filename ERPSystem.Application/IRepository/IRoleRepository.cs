using ERPSystem.Application.DTOs.AppRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.IRepository
{
    public interface IRoleRepository
    {
        Task<bool> IsRoleAssignedToAnyUserAsync(int Id);
        Task<List<GetPermissionDTO>> GetAllPermissionsAsync();
    }
}
