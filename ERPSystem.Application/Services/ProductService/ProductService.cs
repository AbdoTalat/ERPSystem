using AutoMapper;
using ERPSystem.Domain;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using ERPSystem.Domain.Entities;
using ERPSystem.Application.DTOs.Product;
using ERPSystem.Application.DTOs.Employee;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetProductDTO>>> GetAllProductsAsync()
        {
            var departments = await _unitOfWork.Repository<Product>().GetAllAsDtoAsync<GetProductDTO>();

            return ApiResponseHelper<IEnumerable<GetProductDTO>>.ResponseSuccess(data: departments);

        }
        public async Task<ApiResponseHelper<GetProductDTO>> GetProductByIdAsync(int Id)
        {
            var department = await _unitOfWork.Repository<Product>().GetByIdAsDtoAsync<GetProductDTO>(Id);

            if (department == null)
            {
                return ApiResponseHelper<GetProductDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Product Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetProductDTO>.ResponseSuccess(data: department);
        }

        public async Task<ApiResponseHelper<IEnumerable<GetProductDTO>>> GetProductsByCategoryIdAsync(int CategoryId)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllAsDtoAsync<GetProductDTO>(e => e.CategoryId == CategoryId);

            return ApiResponseHelper<IEnumerable<GetProductDTO>>.ResponseSuccess(data: products);
        }
        public async Task<ApiResponseHelper<GetProductDTO>> AddProductAsync(ProductDTO departmentDTO)
        {
            try
            {
                var departmentToAdd = _mapper.Map<Product>(departmentDTO);
                await _unitOfWork.Repository<Product>().AddNewAsync(departmentToAdd);
                await _unitOfWork.CommitAsync();

                var addedProduct = await _unitOfWork.Repository<Product>().GetByIdAsDtoAsync<GetProductDTO>(departmentToAdd.Id);

                return ApiResponseHelper<GetProductDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Product Created Successfully", addedProduct);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetProductDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetProductDTO>> EditProductAsync(int Id, ProductDTO model)
        {
            var oldProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (oldProduct == null)
            {
                return ApiResponseHelper<GetProductDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Product Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(model, oldProduct);

                _unitOfWork.Repository<Product>().Update(oldProduct);
                await _unitOfWork.CommitAsync();

                var updatedProduct = await _unitOfWork.Repository<Product>().GetByIdAsDtoAsync<GetProductDTO>(Id);

                return ApiResponseHelper<GetProductDTO>.ResponseSuccess(message: "Product Updated Successfully.", data: updatedProduct);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetProductDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteProductAsync(int Id)
        {
            var department = await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (department == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Product Not found with ID: {Id}");
            }

            try
            {
                _unitOfWork.Repository<Product>().Delete(department);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message: $"{department.Name} Product Deleted Successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    "Cannot delete product because it is referenced by another record."
                );
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
