using ERPSystem.Application.Services.BranchManagement;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities.Auth;
using Helper.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using ERPSystem.Application.DTOs.Account;

namespace ERPSystem.Application.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            IConfiguration configuration,
            SignInManager<AppUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateAccessTokenAsync(AppUser user, List<string> roles)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var authClaims = principal.Claims.ToList();

            //authClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var jti = Guid.NewGuid().ToString();
            authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var authSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
        public async Task<ApiResponseHelper<LoginResultDTO>> RefreshTokenAsync(RefreshTokenDTO dto)
        {
            var storedToken = await _unitOfWork.Repository<RefreshToken>()
                .FirstOrDefaultAsync(x => x.Token == dto.RefreshToken && !x.IsRevoked && !x.IsUsed);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
                return ApiResponseHelper<LoginResultDTO>.ResponseFailure(StatusCodes.UNAUTHORIZED, "Invalid or expired refresh token.");

            var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(storedToken.UserId);
            if (user == null)
                return ApiResponseHelper<LoginResultDTO>.ResponseFailure(StatusCodes.UNAUTHORIZED, "User not found.");

            var roles = (await _signInManager.UserManager.GetRolesAsync(user)).ToList();
            var newAccessToken = await GenerateAccessTokenAsync(user, roles);
            var newRefreshToken = await GenerateRefreshTokenAsync();

            storedToken.IsUsed = true;

            var newToken = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.Repository<RefreshToken>().AddNewAsync(newToken);
            await _unitOfWork.CommitAsync();

            return ApiResponseHelper<LoginResultDTO>.ResponseSuccess(StatusCodes.OK, "Token refreshed successfully.",
                new LoginResultDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserId = user.Id
                });
        }
        public async Task<ApiResponseHelper<string>> RevokeRefreshTokenAsync(int userId)
        {
            var tokens = await _unitOfWork.Repository<RefreshToken>()
                .GetAllAsync(x => x.UserId == userId && !x.IsRevoked);

            foreach (var token in tokens)
                token.IsRevoked = true;

            _unitOfWork.Repository<RefreshToken>().UpdateRange(tokens);
            await _unitOfWork.CommitAsync();

            await _signInManager.SignOutAsync();

            return ApiResponseHelper<string>.ResponseSuccess(StatusCodes.OK, "Logged out successfully.", "Logout successful");
        }
    }

}
