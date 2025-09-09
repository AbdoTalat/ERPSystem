using ERPSystem.Application.DTOs.AppUser;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.UserService
{
    public interface IUserService
    {
        Task<ApiResponseHelper<IEnumerable<GetAllUsersDTO>>> GetAllUsersAsync();
        Task<ApiResponseHelper<GetUserByIdDTO>> GetUserByIdAsync(int Id);
        Task<ApiResponseHelper<GetUserBranchesDTO>> GetUserBranchesByUserIdAsync(int userId);
        Task<ApiResponseHelper<GetUserByIdDTO>> AddUserAsync(AddUserDTO userDTO);
        Task<ApiResponseHelper<GetUserByIdDTO>> EditUserAsync(int Id, EditUserDTO userDTO);
        Task<ApiResponseHelper<string>> DeleteUserByIdAsync(int Id);
        Task<ApiResponseHelper<string>> AssignRolesToUSerAsync(int userId, AssignRolesToUserDTO rolesToUserDTO);
        Task<ApiResponseHelper<object>> ChangeDefaultBranch(int userId, int branchId);
    }
}
