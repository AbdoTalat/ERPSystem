using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.Services.GoodsReceiptService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsReceiptController : BaseController
    {
        private readonly IGoodsReceiptService _goodsReceiptService;

        public GoodsReceiptController(IGoodsReceiptService goodsReceiptService)
        {
            _goodsReceiptService = goodsReceiptService;
        }

        [HttpPost("Purchase-Order/{purchaseOrderId}")]
        [Authorize(Policy = "PurchaseOrder.ReceiveGoods")]
        public async Task<IActionResult> GoodsReceipt([FromRoute] int purchaseOrderId, [FromQuery] int warehouseId, [FromBody] ReceiveGoodsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }
            var result = await _goodsReceiptService.ReceiveGoodsAsync(purchaseOrderId, warehouseId, dto, UserId);

            return StatusCode(result.StatusCode, result);
        }

    }
}
