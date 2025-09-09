using ERPSystem.Application.DTOs.Customer;
using ERPSystem.Application.Services.CustomerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        [Authorize(Policy = "Customer.View")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomersAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Customer.View")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int Id)
        {
            var result = await _customerService.GetCustomerByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Customer.Add")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _customerService.AddCustomerAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Customer.Edit")]
        public async Task<IActionResult> EditCustomer([FromRoute] int Id, [FromBody] CustomerDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _customerService.EditCustomerAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Customer.Delete")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int Id)
        {
            var result = await _customerService.DeleteCustomerAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

    }
}
