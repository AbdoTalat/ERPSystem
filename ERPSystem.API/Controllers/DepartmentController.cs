using ERPSystem.Application.DTOs.Department;
using ERPSystem.Application.Services.DepartmentService;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize(Policy = "Department.View")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Department.View")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int Id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Department.Add")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new {Error = ModelState});
            }

            var result = await _departmentService.AddDepartmentAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Department.Edit")]
        public async Task<IActionResult> EditDepartment([FromRoute] int Id, [FromBody] DepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentService.EditDepartmentAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Department.Delete")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int Id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
