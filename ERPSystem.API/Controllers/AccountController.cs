using ERPSystem.Application.DTOs.Account;
using ERPSystem.Application.Services.AccountService;
using ERPSystem.Application.Services.BranchManagement;
using ERPSystem.Application.Services.TokenService;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private int UserId => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int UserId) ? UserId : 0;
        
        private readonly IAccountService _accountService;
        private readonly IBranchService _branchService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, 
            IBranchService branchService, ITokenService tokenService)
        {
            _accountService = accountService;
            _branchService = branchService;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _accountService.LoginAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Change-Password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordDTO model)
        {
            int userId = UserId;
            var result = await _accountService.ChangeUserPasswordAsync(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            int userId = UserId;
            var result = await _tokenService.RevokeRefreshTokenAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var result = await _tokenService.RefreshTokenAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            int userId = UserId;
            var result = await _accountService.GetUserProfileAsync(userId);
            return StatusCode(result.StatusCode, result);
        }


    }
}
