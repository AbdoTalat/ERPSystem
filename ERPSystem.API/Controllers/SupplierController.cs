using ERPSystem.Application.DTOs.Supplier;
using ERPSystem.Application.Services.SupplierService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [Authorize(Policy = "Supplier.View")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var result = await _supplierService.GetAllSuppliersAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Supplier.View")]
        public async Task<IActionResult> GetSupplierById([FromRoute] int Id)
        {
            var result = await _supplierService.GetSupplierByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Supplier.Add")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _supplierService.AddSupplierAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Supplier.Edit")]
        public async Task<IActionResult> EditSupplier([FromRoute] int Id, [FromBody] SupplierDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _supplierService.EditSupplierAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Supplier.Delete")]
        public async Task<IActionResult> DeleteSupplier([FromRoute] int Id)
        {
            var result = await _supplierService.DeleteSupplierAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
