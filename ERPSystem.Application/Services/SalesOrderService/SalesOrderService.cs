using AutoMapper;
using ERPSystem.Application.DTOs.SalesOrder;
using ERPSystem.Application.DTOs.Stock;
using ERPSystem.Application.IRepository;
using ERPSystem.Application.Services.StockService;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Enums;
using Helper.API;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.SalesOrderService
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;
        private readonly IProductRepository _productRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public SalesOrderService(IUnitOfWork unitOfWork, IMapper mapper,
            IStockService stockService,
            IProductRepository productRepository,
            ISalesOrderRepository salesOrderRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stockService = stockService;
            _productRepository = productRepository;
            _salesOrderRepository = salesOrderRepository;
        }
        public async Task<ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>> GetAllSalesOrderAsync()
        {
            var so = await _unitOfWork.Repository<SalesOrder>().GetAllAsDtoAsync<GetSalesOrderDTO>();

            return ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>.ResponseSuccess(data: so);
        }
        public async Task<ApiResponseHelper<GetSalesOrderDTO>> GetSalesOrderByIdAsync(int Id)
        {
            var so = await _unitOfWork.Repository<SalesOrder>().GetByIdAsDtoAsync<GetSalesOrderDTO>(Id);
            if (so != null)
            {
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(data: so);
            }
            return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Purchase Orcer not found with ID: {Id}");
        }
        public async Task<ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>> GetSalesOrdersByCustomerIdAsync(int customerId)
        {
            var po = await _unitOfWork.Repository<SalesOrder>()
                .GetAllAsDtoAsync<GetSalesOrderDTO>(po => po.CustomerId == customerId);

            return ApiResponseHelper<IEnumerable<GetSalesOrderDTO>>.ResponseSuccess(data: po);
        }
        public async Task<ApiResponseHelper<GetSalesOrderDTO>> AddSalesOrderAsync(SalesOrderDTO dto)
        {
            var isCustomerExist = await _unitOfWork.Repository<Customer>().IsExistsAsync(c => c.Id == dto.CustomerId && c.IsActive);
            if (!isCustomerExist)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Customer not found or not active.");
            
            if (!dto.Lines.Any())
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Order must have at least one line.");
            
                var productIDs = dto.Lines.Select(l => l.ProductId).ToList();
                var invalidProductIds = await _productRepository.CheckProductsExistById(productIDs);

                if (invalidProductIds.Any())
                    return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                        $"Invalid Product IDs: {string.Join(", ", invalidProductIds)}");

                var orderToAdd = new SalesOrder
                {
                    CustomerId = dto.CustomerId,
                    OrderDate = dto.OrderDate,
                    Status = SalesOrderStatus.Pending,
                    SalesOrderLines = new List<SalesOrderLine>()
                };

                var warehouseIDs = dto.Lines.Select(l => l.WarehouseId).ToList();
                var stocks = await _unitOfWork.Repository<Stock>().GetAllAsync(s => productIDs.Contains(s.ProductId)
                        && warehouseIDs.Contains(s.WarehouseId), includes: s => s.Product);

                var StockDict = stocks.ToDictionary(s => (s.ProductId, s.WarehouseId));
                foreach (var line in dto.Lines)
                {
                    if (!StockDict.TryGetValue((line.ProductId, line.WarehouseId), out var stock) || stock.AvailableQuantity < line.Quantity)
                    {
                        return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                            $"Insufficient stock for Product {line.ProductId} in Warehouse {line.WarehouseId}.");
                    }
                    var orderLine = new SalesOrderLine
                    {
                        SalesOrder = orderToAdd,
                        WarehouseId = line.WarehouseId,
                        ProductId = line.ProductId,
                        Quantity = line.Quantity,
                        UnitPrice = stock.Product.Price,
                        LineTotal = line.Quantity * stock.Product.Price
                    };

                    orderToAdd.SalesOrderLines.Add(orderLine);
                }
            try
            {
                orderToAdd.TotalAmount = orderToAdd.SalesOrderLines.Sum(l => l.LineTotal);

                await _unitOfWork.Repository<SalesOrder>().AddNewAsync(orderToAdd);
                await _unitOfWork.CommitAsync();

                var addedOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsDtoAsync<GetSalesOrderDTO>(orderToAdd.Id);

                return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(StatusCodes.CREATED, "New Sales Order created successfully.", addedOrder);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.InnerException.Message);
            }
        }
        public async Task<ApiResponseHelper<GetSalesOrderDTO>> EditSalesOrderByIdAsync(int Id, SalesOrderDTO dto)
        {
            var oldSalesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(Id);
            if (oldSalesOrder == null)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Sales Order Not found with ID: {Id}");
            
            if (!dto.Lines.Any())
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Order must have at least one line.");
            
            var productIDs = dto.Lines.Select(l => l.ProductId).ToList();
            var invalidProductIds = await _productRepository.CheckProductsExistById(productIDs);

            if (invalidProductIds.Any())
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    $"Invalid Product IDs: {string.Join(", ", invalidProductIds)}");
            
            try
            {
                var poLines = await _unitOfWork.Repository<SalesOrderLine>().GetAllAsync(pol => pol.SalesOrderId == oldSalesOrder.Id);

                _unitOfWork.Repository<SalesOrderLine>().DeleteRange(poLines);

                _mapper.Map(dto, oldSalesOrder);
                oldSalesOrder.TotalAmount = oldSalesOrder.SalesOrderLines.Sum(l => l.LineTotal);

                _unitOfWork.Repository<SalesOrder>().Update(oldSalesOrder);
                await _unitOfWork.CommitAsync();

                var updatedPO = await _unitOfWork.Repository<SalesOrder>().GetByIdAsDtoAsync<GetSalesOrderDTO>(Id);

                return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(message: "Purchase Order Updated Successfully.", data: updatedPO);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetSalesOrderDTO>> ConfirmSalesOrderByIdAsync(int id)
        {
            var salesOrder = await _unitOfWork.Repository<SalesOrder>()
                .FirstOrDefaultAsync(so => so.Id == id, includes: q => q.SalesOrderLines);

            if (salesOrder == null)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Sales Order not found.");

            if (salesOrder.Status != SalesOrderStatus.Pending)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Pending Sales Orders can be Confirmed.");

            try
            {
                foreach (var line in salesOrder.SalesOrderLines)
                {
                    var stock = await _unitOfWork.Repository<Stock>()
                        .FirstOrDefaultAsync(s => s.ProductId == line.ProductId && s.WarehouseId == line.WarehouseId);

                    if (stock == null || stock.Quantity - stock.ReservedQuantity < line.Quantity)
                    {
                        return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                            $"Insufficient available stock for Product {line.ProductId} in Warehouse {line.WarehouseId}.");
                    }

                    stock.ReservedQuantity += line.Quantity;
                    _unitOfWork.Repository<Stock>().Update(stock);
                }

                salesOrder.Status = SalesOrderStatus.Confirmed;
                _unitOfWork.Repository<SalesOrder>().Update(salesOrder);

                await _unitOfWork.CommitAsync();

                var dto = await _unitOfWork.Repository<SalesOrder>().GetByIdAsDtoAsync<GetSalesOrderDTO>(salesOrder.Id);
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(StatusCodes.OK, "Sales Order confirmed and stock reserved.", dto);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }


        public async Task<ApiResponseHelper<GetSalesOrderDTO>> CompleteSalesOrderAsync(int salesOrderId, int userId)
        {
            var so = await _salesOrderRepository.GetSalesOrderWithSalesOrderLineAsync(salesOrderId);

            if (so == null)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Sales Order not found.");

            if (so.Status != SalesOrderStatus.Confirmed)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Confirmed orders can be completed.");

            try
            {
                foreach (var line in so.SalesOrderLines)
                {
                    var decreaseDto = new DecreaseStockDTO
                    {
                        ProductId = line.ProductId,
                        WarehouseId = line.WarehouseId,
                        Quantity = line.Quantity,
                        Reason = $"Sales Order #{so.Id}"
                    };
                    var result = await _stockService.DecreaseStockByIdAsync(userId, decreaseDto, IsCommit: false);
                    if (!result.Success)
                        return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(result.StatusCode, result.Message);
                }
                so.Status = SalesOrderStatus.Completed;
                _unitOfWork.Repository<SalesOrder>().Update(so);

                await _unitOfWork.CommitAsync();

                var newSO = _mapper.Map<GetSalesOrderDTO>(so);
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(StatusCodes.OK, "Sales order completed and stock updated.", newSO);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetSalesOrderDTO>> CancelSalesOrderByIdAsync(int Id)
        {
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(Id);
            if (salesOrder == null)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Purchase Order not found.");

            if (salesOrder.Status != SalesOrderStatus.Pending && salesOrder.Status != SalesOrderStatus.Confirmed)
                return ApiResponseHelper<GetSalesOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Pending or Confirmed Purchase Orders can be Cancelled.");

            salesOrder.Status = SalesOrderStatus.Cancelled;
            _unitOfWork.Repository<SalesOrder>().Update(salesOrder);
            await _unitOfWork.CommitAsync();

            var dto = await _unitOfWork.Repository<SalesOrder>().GetByIdAsDtoAsync<GetSalesOrderDTO>(salesOrder.Id);

            return ApiResponseHelper<GetSalesOrderDTO>.ResponseSuccess(StatusCodes.OK, "Purchase Order approved.", dto);
        }
    }
}
