using AutoMapper;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using ERPSystem.Application.DTOs.Supplier;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Services.SupplierService
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetSupplierDTO>>> GetAllSuppliersAsync()
        {
            var suppliers = await _unitOfWork.Repository<Supplier>().GetAllAsDtoAsync<GetSupplierDTO>();

            return ApiResponseHelper<IEnumerable<GetSupplierDTO>>.ResponseSuccess(data: suppliers);

        }
        public async Task<ApiResponseHelper<GetSupplierDTO>> GetSupplierByIdAsync(int Id)
        {
            var Supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsDtoAsync<GetSupplierDTO>(Id);

            if (Supplier == null)
            {
                return ApiResponseHelper<GetSupplierDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Supplier Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetSupplierDTO>.ResponseSuccess(data: Supplier);
        }
        public async Task<ApiResponseHelper<GetSupplierDTO>> AddSupplierAsync(SupplierDTO dto)
        {
            try
            {
                var SupplierToAdd = _mapper.Map<Supplier>(dto);
                await _unitOfWork.Repository<Supplier>().AddNewAsync(SupplierToAdd);
                await _unitOfWork.CommitAsync();

                var addedSupplier = await _unitOfWork.Repository<Supplier>().GetByIdAsDtoAsync<GetSupplierDTO>(SupplierToAdd.Id);

                return ApiResponseHelper<GetSupplierDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Supplier Created Successfully", addedSupplier);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSupplierDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetSupplierDTO>> EditSupplierAsync(int Id, SupplierDTO dto)
        {
            var oldSupplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(Id);
            if (oldSupplier == null)
            {
                return ApiResponseHelper<GetSupplierDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Supplier Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(dto, oldSupplier);

                _unitOfWork.Repository<Supplier>().Update(oldSupplier);
                await _unitOfWork.CommitAsync();

                var updatedSupplier = await _unitOfWork.Repository<Supplier>().GetByIdAsDtoAsync<GetSupplierDTO>(Id);

                return ApiResponseHelper<GetSupplierDTO>.ResponseSuccess(message: "Supplier Updated Successfully.", data: updatedSupplier);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSupplierDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteSupplierAsync(int Id)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(Id);
            if (supplier == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Supplier Not found with ID: {Id}");
            }

            try
            {
                _unitOfWork.Repository<Supplier>().Delete(supplier);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message: $"{supplier.Name} Supplier Deleted Successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.CONFLICT,
                    "Cannot delete supplier because it is referenced by another record."
                );
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
