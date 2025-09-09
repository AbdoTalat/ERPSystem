using ERPSystem.Application.DTOs.Category;
using ERPSystem.Application.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policy = "Category.View")]
        public async Task<IActionResult> GetAllCategorys()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Category.View")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int Id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(Id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Category.Add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _categoryService.AddCategoryAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Category.Edit")]
        public async Task<IActionResult> EditCategory([FromRoute] int Id, [FromBody] CategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _categoryService.EditCategoryAsync(Id, model);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Category.Delete")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int Id)
        {
            var result = await _categoryService.DeleteCategoryAsync(Id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
