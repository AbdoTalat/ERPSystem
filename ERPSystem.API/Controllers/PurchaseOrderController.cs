using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.DTOs.PurchaseOrder;
using ERPSystem.Application.Services.GoodsReceiptService;
using ERPSystem.Application.Services.PurchaseOrderService;
using ERPSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : BaseController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IGoodsReceiptService _goodsReceiptService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, IGoodsReceiptService goodsReceiptService)
        {
            _purchaseOrderService = purchaseOrderService;
            _goodsReceiptService = goodsReceiptService;
        }

        [HttpGet]
        [Authorize(Policy = "PurchaseOrder.View")]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            var result = await _purchaseOrderService.GetAllPurchaseOrderAsync();

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{Id:int}")]
        [Authorize(Policy = "PurchaseOrder.View")]
        public async Task<IActionResult> GetPurchaseOrderById([FromRoute] int Id)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Supplier/{SupplierId:int}")]
        [Authorize(Policy = "PurchaseOrder.View")]
        public async Task<IActionResult> GetPurchaseOrderBySupplierId([FromRoute] int SupplierId)
        {
            var result = await _purchaseOrderService.GetPurchaseOrdersBySupplierIdAsync(SupplierId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "PurchaseOrder.Add")]
        public async Task<IActionResult> AddPurchaseOrders([FromBody] PurchaseOrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _purchaseOrderService.AddPurchaseOrderAsync(model);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{Id:int}")]
        [Authorize(Policy = "PurchaseOrder.Edit")]
        public async Task<IActionResult> EditPurchaseOrder([FromRoute] int Id, [FromBody] PurchaseOrderDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _purchaseOrderService.EditPurchaseOrderByIdAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("{Id:int}/Approve")]
        [Authorize(Policy = "PurchaseOrder.Approve")]
        public async Task<IActionResult> ApprovePurchaseOrder([FromRoute] int Id)
        {
            var result = await _purchaseOrderService.ApprovePurchaseOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}/Cancell")]
        [Authorize(Policy = "PurchaseOrder.Cancell")]
        public async Task<IActionResult> CancellPurchaseOrder([FromRoute] int Id)
        {
            var result = await _purchaseOrderService.CancelPurchaseOrderByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }

}