using ERPSystem.Application.DTOs.Category;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ApiResponseHelper<IEnumerable<GetCategoryDTO>>> GetAllCategoriesAsync();
        public Task<ApiResponseHelper<GetCategoryDTO>> GetCategoryByIdAsync(int Id);
        public Task<ApiResponseHelper<GetCategoryDTO>> AddCategoryAsync(CategoryDTO dto);
        public Task<ApiResponseHelper<GetCategoryDTO>> EditCategoryAsync(int Id, CategoryDTO dto);
        public Task<ApiResponseHelper<object>> DeleteCategoryAsync(int Id);
    }
}
