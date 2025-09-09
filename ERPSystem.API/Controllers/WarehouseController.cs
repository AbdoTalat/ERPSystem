using ERPSystem.Application.DTOs.Warehouse;
using ERPSystem.Application.Services.WarehouseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        [Authorize(Policy = "Warehouse.View")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var result = await _warehouseService.GetAllWarehousesAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Warehouse.View")]
        public async Task<IActionResult> GetWarehouseById([FromRoute] int Id)
        {
            var result = await _warehouseService.GetWarehouseByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Warehouse.Add")]
        public async Task<IActionResult> AddWarehouse([FromBody] WarehouseDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _warehouseService.AddWarehouseAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Warehouse.Edit")]
        public async Task<IActionResult> EditWarehouse([FromRoute] int Id, [FromBody] WarehouseDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _warehouseService.EditWarehouseAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Warehouse.Delete")]
        public async Task<IActionResult> DeleteWarehouse([FromRoute] int Id)
        {
            var result = await _warehouseService.DeleteWarehouseAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
