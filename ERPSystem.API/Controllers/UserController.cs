using ERPSystem.Application.DTOs.AppUser;
using ERPSystem.Application.Services.TokenService;
using ERPSystem.Application.Services.UserService;
using ERPSystem.Domain.Entities.Auth;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(
            IUserService userService,
            ITokenService tokenService,
            UserManager<AppUser> userManager)
        {
            _userService = userService;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetUserById([FromRoute] int Id)
        {
            var result = await _userService.GetUserByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{userId:int}/Branches")]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetUserBranchesByUserId([FromRoute] int userId)
        {
            var result = await _userService.GetUserBranchesByUserIdAsync(userId);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        [Authorize(Policy = "User.Add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _userService.AddUserAsync(model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{userId:int}/Assign-Roles")]
        [Authorize(Policy = "User.AssignRoles")]
        public async Task<IActionResult> AssignRolesToUser([FromRoute] int userId, [FromBody] AssignRolesToUserDTO model)
        {
            if (model == null || model.RolesIds == null)
            {
                return BadRequest("Invalid request");
            }

            var result = await _userService.AssignRolesToUSerAsync(userId, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "User.Edit")]
        public async Task<IActionResult> EditUser([FromRoute] int Id, [FromBody] EditUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }
            var result = await _userService.EditUserAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{userId:int}/Default-Branch/{branchId:int}")]
        [Authorize(Policy = "User.Edit")]
        public async Task<IActionResult> ChangeDefaultBranch([FromRoute] int userId, [FromRoute] int branchId)
        {
            var result = await _userService.ChangeDefaultBranch(userId, branchId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "User.Delete")]
        public async Task<IActionResult> DeleteUser([FromRoute] int Id)
        {
            var result = await _userService.DeleteUserByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        
    }
}
