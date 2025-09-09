using ERPSystem.Application.DTOs.SalesOrder;
using ERPSystem.Application.DTOs.SalesOrder;
using ERPSystem.Application.Services.SalesOrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : BaseController
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrderController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        [HttpGet]
        [Authorize(Policy = "SalesOrder.View")]
        public async Task<IActionResult> GetAllSalesOrders()
        {
            var result = await _salesOrderService.GetAllSalesOrderAsync();

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{Id:int}")]
        [Authorize(Policy = "SalesOrder.View")]
        public async Task<IActionResult> GetSalesOrderById([FromRoute] int Id)
        {
            var result = await _salesOrderService.GetSalesOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Customer/{CustomerId:int}")]
        [Authorize(Policy = "SalesOrder.View")]
        public async Task<IActionResult> GetSalesOrderBySupplierId([FromRoute] int CustomerId)
        {
            var result = await _salesOrderService.GetSalesOrdersByCustomerIdAsync(CustomerId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "SalesOrder.Add")]
        public async Task<IActionResult> AddSalesOrder([FromBody] SalesOrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _salesOrderService.AddSalesOrderAsync(model);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{Id:int}")]
        [Authorize(Policy = "SalesOrder.Edit")]
        public async Task<IActionResult> EditSalesOrder([FromRoute] int Id, [FromBody] SalesOrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _salesOrderService.EditSalesOrderByIdAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{Id:int}/Complete")]
        [Authorize(Policy = "SalesOrder.Complete")]
        public async Task<IActionResult> CompleteSalesOrder([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _salesOrderService.CompleteSalesOrderAsync(Id, UserId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}/Confirm")]
        [Authorize(Policy = "SalesOrder.Confirm")]
        public async Task<IActionResult> ConfirmSalesOrder([FromRoute] int Id)
        {
            var result = await _salesOrderService.ConfirmSalesOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}/Cancell")]
        [Authorize(Policy = "SalesOrder.Cancell")]
        public async Task<IActionResult> CancellSalesOrder([FromRoute] int Id)
        {
            var result = await _salesOrderService.CancelSalesOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
