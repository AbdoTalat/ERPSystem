using ERPSystem.Application.DTOs.Payment;
using ERPSystem.Application.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Policy = "Payment.View")]
        public async Task<IActionResult> GetAllPaymentes()
        {
            var result = await _paymentService.GetAllPaymentsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Payment.View")]
        public async Task<IActionResult> GetPaymentById([FromRoute] int Id)
        {
            var result = await _paymentService.GetPaymentByIdAsync(Id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Invoice/{InvoiceId:int}")]
        [Authorize(Policy = "Payment.View")]
        public async Task<IActionResult> GetPaymentByInvoiceId([FromRoute] int InvoiceId)
        {
            var result = await _paymentService.GetPaymentsByInvoiceIdAsync(InvoiceId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Invoice/{InvoiceId:int}")]
        [Authorize(Policy = "Payment.Add")]
        public async Task<IActionResult> AddPayment([FromRoute] int InvoiceId, PaymentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _paymentService.AddPaymentAsync(InvoiceId, model);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{PaymentId:int}")]
        [Authorize(Policy = "Payment.Edit")]
        public async Task<IActionResult> EditPayment([FromRoute] int PaymentId, PaymentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _paymentService.EditPaymentAsync(PaymentId, model);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{PaymentId:int}/Refund")]
        [Authorize(Policy = "Payment.Refund")]
        public async Task<IActionResult> RefundPayment([FromRoute] int PaymentId, [FromQuery] decimal refundAmount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _paymentService.RefundPaymentAsync(PaymentId, refundAmount);
            return StatusCode(result.StatusCode, result);
        }

    }
}
