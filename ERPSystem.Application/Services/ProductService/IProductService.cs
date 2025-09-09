using ERPSystem.Application.DTOs.Employee;
using ERPSystem.Application.DTOs.Product;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponseHelper<IEnumerable<GetProductDTO>>> GetAllProductsAsync();
        Task<ApiResponseHelper<GetProductDTO>> GetProductByIdAsync(int Id);
        Task<ApiResponseHelper<IEnumerable<GetProductDTO>>> GetProductsByCategoryIdAsync(int CategoryId);
        Task<ApiResponseHelper<GetProductDTO>> AddProductAsync(ProductDTO departmentDTO);
        Task<ApiResponseHelper<GetProductDTO>> EditProductAsync(int Id, ProductDTO departmentDTO);
        Task<ApiResponseHelper<object>> DeleteProductAsync(int Id);
    }
}
