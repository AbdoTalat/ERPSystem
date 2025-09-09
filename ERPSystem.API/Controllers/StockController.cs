using ERPSystem.Application.DTOs.Stock;
using ERPSystem.Application.DTOs.Warehouse;
using ERPSystem.Application.Services.StockService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : BaseController
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> GetAllStocks()
        {
            var result = await _stockService.GetAllStocksAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> GetStockById(int Id)
        {
            var result = await _stockService.GetStockByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Warehouse/{warehouseId:int}")]
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> GetStockByWarehouse(int warehouseId)
        {
            var result = await _stockService.GetStockByWarehouseAsync(warehouseId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Warehouse/{warehouseId:int}/Product/{productId:int}")]
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> GetStockByProductAndWarehouse(int warehouseId, int productId)
        {
            var result = await _stockService.GetStockByProductAndWarehouseAsync(warehouseId, productId);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        [Authorize(Policy = "Stock.Add")]
        public async Task<IActionResult> AddStock([FromBody] AddStockDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _stockService.AddStockAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Increase")]
        [Authorize(Policy = "Stock.Edit")]
        public async Task<IActionResult> IncreaseStock([FromBody] IncreaseStockDTO Smodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _stockService.IncreaseStockByIdAsync(UserId, Smodel, IsCommit: true);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Decrease")]
        [Authorize(Policy = "Stock.Edit")]
        public async Task<IActionResult> DecreaseStock( [FromBody] DecreaseStockDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _stockService.DecreaseStockByIdAsync(UserId, model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Transfer")]
        [Authorize(Policy = "Stock.Transfer")]
        public async Task<IActionResult> TransferStock([FromBody] TransferStockDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _stockService.TransferStockAsync(UserId, model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Stock.Delete")]
        public async Task<IActionResult> DeleteStock([FromRoute] int Id)
        {
            var result = await _stockService.DeleteStockByIdAsync(Id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
