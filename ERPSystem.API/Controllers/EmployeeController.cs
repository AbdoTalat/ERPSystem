using ERPSystem.Application.DTOs.Employee;
using ERPSystem.Application.Services.EmployeeService;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Policy = "Employee.View")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _employeeService.GetAllEmployeesAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Employee.View")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int Id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Department/{departmentId:int}")]
        [Authorize(Policy = "Employee.View")]
        public async Task<IActionResult> GetEmployeeByDepartmentIdId([FromRoute] int departmentId)
        {
            var result = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        [Authorize(Policy = "Employee.Add")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.AddEmployeeAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Employee.Edit")]
        public async Task<IActionResult> EditEmployee([FromRoute] int Id, [FromBody] EmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }
            var result = await _employeeService.EditEmployeeAsync(model, Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Employee.Delete")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int Id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(Id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
