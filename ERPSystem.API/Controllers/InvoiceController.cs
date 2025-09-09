using ERPSystem.Application.DTOs.Invoice;
using ERPSystem.Application.Services.InvoiceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        [Authorize(Policy = "Invoice.View")]
        public async Task<IActionResult> GetAllInvoicees()
        {
            var result = await _invoiceService.GetAllInvoicesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Invoice.View")]
        public async Task<IActionResult> GetInvoiceById([FromRoute] int Id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(Id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Sales-Order/{SalesOrderId:int}")]
        [Authorize(Policy = "Invoice.Add")]
        public async Task<IActionResult> AddInvoice([FromRoute] int SalesOrderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _invoiceService.AddInvoiceAsync(SalesOrderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}/Cancell")]
        [Authorize(Policy = "Invoice.Cancell")]
        public async Task<IActionResult> CancellInvoice([FromRoute] int Id)
        {
            var result = await _invoiceService.CancellInvoiceByIdAsync(Id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Invoice.Delete")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int Id)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(Id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
