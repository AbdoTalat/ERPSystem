using AutoMapper;
using ERPSystem.Application.DTOs.Category;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using Helper.API;
using Helper.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetCategoryDTO>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsDtoAsync<GetCategoryDTO>();

            return ApiResponseHelper<IEnumerable<GetCategoryDTO>>.ResponseSuccess(data: categories);

        }
        public async Task<ApiResponseHelper<GetCategoryDTO>> GetCategoryByIdAsync(int Id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsDtoAsync<GetCategoryDTO>(Id);

            if (category == null)
            {
                return ApiResponseHelper<GetCategoryDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Category Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetCategoryDTO>.ResponseSuccess(data: category);
        }
        public async Task<ApiResponseHelper<GetCategoryDTO>> AddCategoryAsync(CategoryDTO dto)
        {
            try
            {
                var categoryToAdd = _mapper.Map<Category>(dto);
                await _unitOfWork.Repository<Category>().AddNewAsync(categoryToAdd);
                await _unitOfWork.CommitAsync();

                var addedCategory = await _unitOfWork.Repository<Category>().GetByIdAsDtoAsync<GetCategoryDTO>(categoryToAdd.Id);

                return ApiResponseHelper<GetCategoryDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Category Created Successfully", addedCategory);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetCategoryDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.InnerException.Message);
            }
        }
        public async Task<ApiResponseHelper<GetCategoryDTO>> EditCategoryAsync(int Id, CategoryDTO dto)
        {
            var oldCategory = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (oldCategory == null)
            {
                return ApiResponseHelper<GetCategoryDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Category Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(dto, oldCategory);

                _unitOfWork.Repository<Category>().Update(oldCategory);
                await _unitOfWork.CommitAsync();

                var updatedCategory = await _unitOfWork.Repository<Category>().GetByIdAsDtoAsync<GetCategoryDTO>(Id);

                return ApiResponseHelper<GetCategoryDTO>.ResponseSuccess(message: "Category Updated Successfully.", data: updatedCategory);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetCategoryDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteCategoryAsync(int Id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (category == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Category Not found with ID: {Id}");
            }

            try
            {
                _unitOfWork.Repository<Category>().Delete(category);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message: $"{category.Name} Category Deleted Successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.CONFLICT, "Cannot delete category because it is referenced by another record.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
