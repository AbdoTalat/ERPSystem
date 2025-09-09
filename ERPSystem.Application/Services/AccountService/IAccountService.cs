using ERPSystem.Application.DTOs.Account;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponseHelper<LoginResultDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ApiResponseHelper<string>> ChangeUserPasswordAsync(int userId, ChangeUserPasswordDTO passwordDTO);
        Task<ApiResponseHelper<GetUserProfileDTO>> GetUserProfileAsync(int userId);
    }
}
