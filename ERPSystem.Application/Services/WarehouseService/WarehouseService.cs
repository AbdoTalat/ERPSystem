using AutoMapper;
using ERPSystem.Application.DTOs.Warehouse;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using Helper.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;

namespace ERPSystem.Application.Services.WarehouseService
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetWarehousesDTO>>> GetAllWarehousesAsync()
        {
            var warehouses = await _unitOfWork.Repository<Warehouse>().GetAllAsDtoAsync<GetWarehousesDTO>();

            return ApiResponseHelper<IEnumerable<GetWarehousesDTO>>.ResponseSuccess(data: warehouses);

        }
        public async Task<ApiResponseHelper<GetWarehousesDTO>> GetWarehouseByIdAsync(int Id)
        {
            var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsDtoAsync<GetWarehousesDTO>(Id);

            if (warehouse == null)
            {
                return ApiResponseHelper<GetWarehousesDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Warehouse Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetWarehousesDTO>.ResponseSuccess(data: warehouse);
        }
        public async Task<ApiResponseHelper<GetWarehousesDTO>> AddWarehouseAsync(WarehouseDTO dto)
        {
            try
            {
                var warehouseToAdd = _mapper.Map<Warehouse>(dto);
                await _unitOfWork.Repository<Warehouse>().AddNewAsync(warehouseToAdd);
                await _unitOfWork.CommitAsync();

                var addedWarehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsDtoAsync<GetWarehousesDTO>(warehouseToAdd.Id);

                return ApiResponseHelper<GetWarehousesDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Warehouse Created Successfully", addedWarehouse);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetWarehousesDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.InnerException.Message);
            }
        }
        public async Task<ApiResponseHelper<GetWarehousesDTO>> EditWarehouseAsync(int Id, WarehouseDTO dto)
        {
            var oldWarehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(Id);
            if (oldWarehouse == null)
            {
                return ApiResponseHelper<GetWarehousesDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Warehouse Not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(dto, oldWarehouse);

                _unitOfWork.Repository<Warehouse>().Update(oldWarehouse);
                await _unitOfWork.CommitAsync();

                var updatedWarehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsDtoAsync<GetWarehousesDTO>(Id);

                return ApiResponseHelper<GetWarehousesDTO>.ResponseSuccess(message: "Warehouse Updated Successfully.", data: updatedWarehouse);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetWarehousesDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<string>> DeleteWarehouseAsync(int Id)
        {
            var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(Id);
            if (warehouse == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"Warehouse Not found with ID: {Id}");
            }

            try
            {
                _unitOfWork.Repository<Warehouse>().Delete(warehouse);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<string>.ResponseSuccess(message: $"{warehouse.Name} Warehouse Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
