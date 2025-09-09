using ERPSystem.Application.DTOs.Account;
using ERPSystem.Application.Services.TokenService;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities.Auth;
using Helper.API;
using Helper.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseHelper<LoginResultDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return ApiResponseHelper<LoginResultDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Invalid email address.");

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                return ApiResponseHelper<LoginResultDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Incorrect password.");

            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = await _tokenService.GenerateAccessTokenAsync(user, roles.ToList());
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

                var refreshEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };

                await _unitOfWork.Repository<RefreshToken>().AddNewAsync(refreshEntity);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<LoginResultDTO>.ResponseSuccess(message: "Login successful.", data: new LoginResultDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<LoginResultDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<ApiResponseHelper<string>> ChangeUserPasswordAsync(int userId, ChangeUserPasswordDTO passwordDTO)
        {
            if (userId == 0)
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.UNAUTHORIZED, "User not authorized.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"User not found with ID: {userId}");

            if (!await _userManager.CheckPasswordAsync(user, passwordDTO.CurrentPassword))
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, "Current password is incorrect.");

            if (passwordDTO.CurrentPassword == passwordDTO.NewPassword)
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, "New password cannot be the same as the old password.");

            var result = await _userManager.ChangePasswordAsync(user, passwordDTO.CurrentPassword, passwordDTO.NewPassword);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, errorMessages);
            }

            return ApiResponseHelper<string>.ResponseSuccess(message: "User password changed successfully.");
        }
        public async Task<ApiResponseHelper<GetUserProfileDTO>> GetUserProfileAsync(int userId)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetByIdAsDtoAsync<GetUserProfileDTO>(userId);
            if (user == null)
            {
                return ApiResponseHelper<GetUserProfileDTO>.ResponseFailure(StatusCodes.UNAUTHORIZED, "User not Authorized.");
            }

            return ApiResponseHelper<GetUserProfileDTO>.ResponseSuccess(data: user);
        }
    }

}
