using AutoMapper;
using ERPSystem.Application.DTOs.Stock;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using Helper.API;
using Helper.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.StockService
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponseHelper<IEnumerable<GetStockDTO>>> GetAllStocksAsync()
        {
            var stocks = await _unitOfWork.Repository<Stock>()
                .GetAllAsDtoAsync<GetStockDTO>();

            return ApiResponseHelper< IEnumerable<GetStockDTO>>.ResponseSuccess(data: stocks);
        }
        public async Task<ApiResponseHelper<GetStockDTO>> GetStockByIdAsync(int Id)
        {
            var stock = await _unitOfWork.Repository<Stock>()
               .GetByIdAsDtoAsync<GetStockDTO>(Id);
            if (stock != null)
            {
                return ApiResponseHelper<GetStockDTO>.ResponseSuccess(data: stock);
            }
            return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"No stock founs with this ID: {Id}");
        }
        public async Task<ApiResponseHelper<IEnumerable<GetStockDTO>>> GetStockByProductAndWarehouseAsync(int warehouseId, int productId)
        {
            var stocks = await _unitOfWork.Repository<Stock>()
                .GetAllAsDtoAsync<GetStockDTO>(s => s.WarehouseId == warehouseId && s.ProductId == productId);

            if(stocks.Any())
            {
                return ApiResponseHelper<IEnumerable<GetStockDTO>>.ResponseSuccess(data: stocks);
            }
            return ApiResponseHelper<IEnumerable<GetStockDTO>>.ResponseFailure(StatusCodes.NOT_FOUND, "No stock founs with this");
        }
        public async Task<ApiResponseHelper<IEnumerable<GetStockByWarehouseDTO>>> GetStockByWarehouseAsync(int warehouseId)
        {
            var stocks = await _unitOfWork.Repository<Stock>()
                .GetAllAsDtoAsync<GetStockByWarehouseDTO>(s => s.WarehouseId == warehouseId);

            return ApiResponseHelper<IEnumerable<GetStockByWarehouseDTO>>.ResponseSuccess(data: stocks);
        }
        public async Task<ApiResponseHelper<GetStockDTO>> AddStockAsync(AddStockDTO dto)
        {
            try
            {
                var stockToAdd = _mapper.Map<Stock>(dto);

                await _unitOfWork.Repository<Stock>().AddNewAsync(stockToAdd);
                await _unitOfWork.CommitAsync();

                var addedStock = await _unitOfWork.Repository<Stock>().GetByIdAsDtoAsync<GetStockDTO>(stockToAdd.Id);

                return ApiResponseHelper<GetStockDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "New Stock added successfully.", addedStock);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetStockDTO>
                    .ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.InnerException.Message);
            }
        }
        public async Task<ApiResponseHelper<GetStockDTO>> IncreaseStockByIdAsync(IncreaseStockDTO dto, bool IsCommit = true)
        {
            if (dto.Quantity <= 0)
                return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Quantity must be greater than zero.");

            try
            {
                var stock = await _unitOfWork.Repository<Stock>()
                .FirstOrDefaultAsync(s => s.ProductId == dto.ProductId && s.WarehouseId == dto.WarehouseId);

                if (stock == null)
                {
                    stock = _mapper.Map<Stock>(dto);
                    await _unitOfWork.Repository<Stock>().AddNewAsync(stock);
                }
                else
                {
                    stock.Quantity += dto.Quantity;
                    stock.DamagedQuantity += dto.DamagedQuantity;
                    _unitOfWork.Repository<Stock>().Update(stock);
                }
                var stockMovement = new StockMovement
                {
                    Stock = stock,
                    StockId = stock.Id,
                    ProductId = dto.ProductId,
                    WarehouseId = dto.WarehouseId,
                    Quantity = dto.Quantity,
                    Reason = dto.Reason
                };

                await _unitOfWork.Repository<StockMovement>().AddNewAsync(stockMovement);

                if (IsCommit)
                    await _unitOfWork.CommitAsync();

                var updatedStock = _mapper.Map<GetStockDTO>(stock);

                return ApiResponseHelper<GetStockDTO>.ResponseSuccess(StatusCodes.OK, "Stock Increased Successfully.", updatedStock);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetStockDTO>> DecreaseStockByIdAsync(DecreaseStockDTO dto, bool IsCommit = true)
        {
            try
            {
                var stock = await _unitOfWork.Repository<Stock>()
                .FirstOrDefaultAsync(s => s.ProductId == dto.ProductId && s.WarehouseId == dto.WarehouseId);

                if (stock == null)
                {
                    return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "No Stock for this product in this warhouse.");
                }
                if (stock.Quantity <= 0)
                {
                    return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Stock is empty.");
                }
                if (dto.Quantity > stock.Quantity)
                {
                    return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, $"Insufficient stock, stock quantity is: {stock.Quantity} and you want to remove {dto.Quantity}");
                } 
                stock.Quantity -= dto.Quantity;
                stock.ReservedQuantity -= dto.Quantity;
                _unitOfWork.Repository<Stock>().Update(stock);

                var stockMovement = _mapper.Map<StockMovement>(dto);

                stockMovement.Quantity = -dto.Quantity;
                stockMovement.StockId = stock.Id;
                stockMovement.ProductId = stock.ProductId;
                stockMovement.WarehouseId = stock.WarehouseId;

                await _unitOfWork.Repository<StockMovement>().AddNewAsync(stockMovement);
                if(IsCommit)
                    await _unitOfWork.CommitAsync();

                var updatedStock = _mapper.Map<GetStockDTO>(stock);

                return ApiResponseHelper<GetStockDTO>.ResponseSuccess(StatusCodes.OK, "Stock Decreased Successfully.", updatedStock);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetStockDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> TransferStockAsync(TransferStockDTO dto)
        {
            if (dto.FromWarehouseId == dto.ToWarehouseId)
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.BAD_REQUEST, "Source and target warehouse cannot be the same.");

            var sourceStock = await _unitOfWork.Repository<Stock>()
                .FirstOrDefaultAsync(s => s.ProductId == dto.ProductId && s.WarehouseId == dto.FromWarehouseId);
            if (sourceStock == null)
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, "Stock not found.");
            
            var IsWarehouseExist = await _unitOfWork.Repository<Warehouse>().AnyAsync(w => w.Id == dto.ToWarehouseId);
            if (!IsWarehouseExist)
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, "Warehouse not found.");
            
            if(dto.Quantity <= 0 || dto.Quantity > sourceStock.AvailableQuantity)
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.BAD_REQUEST, "Invalid quantity for transfer.");

            try
            {
                var targetStock = await _unitOfWork.Repository<Stock>()
                .FirstOrDefaultAsync(s => s.ProductId == sourceStock.ProductId && s.WarehouseId == dto.ToWarehouseId);
                
                if (targetStock == null)
                {
                    targetStock = new Stock
                    {
                        ProductId = sourceStock.ProductId,
                        WarehouseId = dto.ToWarehouseId,
                        Quantity = 0,
                        ReservedQuantity = 0,
                        DamagedQuantity = 0
                    };
                    await _unitOfWork.Repository<Stock>().AddNewAsync(targetStock);
                    await _unitOfWork.CommitAsync();
                }

                sourceStock.Quantity -= dto.Quantity;
                targetStock.Quantity += dto.Quantity;

                _unitOfWork.Repository<Stock>().Update(sourceStock);
                _unitOfWork.Repository<Stock>().Update(targetStock);

                

                await _unitOfWork.Repository<StockMovement>().AddNewAsync(new StockMovement
                {
                    StockId = sourceStock.Id,
                    ProductId = sourceStock.ProductId,
                    WarehouseId = sourceStock.WarehouseId,
                    Quantity = -dto.Quantity,
                    Reason = "Transfer Out"
                });

                await _unitOfWork.Repository<StockMovement>().AddNewAsync(new StockMovement
                {
                    StockId = targetStock.Id,
                    ProductId = targetStock.ProductId,
                    WarehouseId = targetStock.WarehouseId,
                    Quantity = dto.Quantity,
                    Reason = "Transfer In"
                });


                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(StatusCodes.OK, "Stock transfered successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteStockByIdAsync(int Id)
        {
            var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(Id);
            if (stock == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"No stock found with ID: {Id}");
            }
            try
            {
                _unitOfWork.Repository<Stock>().Delete(stock);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message:"Stock deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.CONFLICT,
                    "Cannot delete stock because it is referenced by another record."
                );
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
