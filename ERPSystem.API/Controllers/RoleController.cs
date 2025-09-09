using ERPSystem.Application.DTOs.AppRole;
using ERPSystem.Application.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Policy = "Role.View")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Role.View")]
        public async Task<IActionResult> GetRoleById(int Id)
        {
            var result = await _roleService.GetRoleByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Permissions")]
        [Authorize(Policy = "Role.View")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _roleService.GetAllPermissionsAsync();

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        [Authorize(Policy = "Role.Add")]
        public async Task<IActionResult> AddRole([FromBody] RoleDTO model)
        {
            var result = await _roleService.AddRoleAsync(model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Role.Edit")]
        public async Task<IActionResult> EditRole([FromRoute] int Id, [FromBody] RoleDTO model)
        {
            var result = await _roleService.EditRoleAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Role.Delete")]
        public async Task<IActionResult> DeleteRole([FromRoute] int Id)
        {
            var result = await _roleService.DeleteRoleAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
