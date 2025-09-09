using ERPSystem.Application.DTOs.Account;
using ERPSystem.Domain.Entities.Auth;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(AppUser user, List<string> roles);
        Task<string> GenerateRefreshTokenAsync();
        Task<ApiResponseHelper<LoginResultDTO>> RefreshTokenAsync(RefreshTokenDTO dto);
        Task<ApiResponseHelper<string>> RevokeRefreshTokenAsync(int userId);
    }
}
