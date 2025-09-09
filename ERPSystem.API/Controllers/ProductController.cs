using ERPSystem.Application.DTOs.Product;
using ERPSystem.Application.Services.ProductService;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> GetProductById([FromRoute] int Id)
        {
            var result = await _productService.GetProductByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Category/{categoryId:int}")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> GetEmployeeByDepartmentIdId([FromRoute] int categoryId)
        {
            var result = await _productService.GetProductsByCategoryIdAsync(categoryId);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Add")]
        [Authorize(Policy = "Product.Add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = ModelState });
            }

            var result = await _productService.AddProductAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Edit/{Id:int}")]
        [Authorize(Policy = "Product.Edit")]
        public async Task<IActionResult> EditProduct([FromRoute] int Id, [FromBody] ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.EditProductAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Product.Delete")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int Id)
        {
            var result = await _productService.DeleteProductAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
