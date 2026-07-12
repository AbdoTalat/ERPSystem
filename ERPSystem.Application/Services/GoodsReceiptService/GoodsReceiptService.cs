using AutoMapper;
using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.DTOs.Stock;
using ERPSystem.Application.IRepository;
using ERPSystem.Application.Services.StockService;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Enums;
using Helper.API;
using Helper.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.GoodsReceiptService
{
    public class GoodsReceiptService : IGoodsReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IStockService _stockService;

        public GoodsReceiptService(IUnitOfWork unitOfWork, IMapper mapper, 
            IPurchaseOrderRepository purchaseOrderRepository, IStockService stockService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _purchaseOrderRepository = purchaseOrderRepository;
            _stockService = stockService;
        }

        public async Task<ApiResponseHelper<GetGoodsReceiptDTO>> ReceiveGoodsAsync(int purchaseOrderId, int warehouseId, ReceiveGoodsDTO dto, int userId)
        {
            var po = await _purchaseOrderRepository.GetPurchaseOrderForReceivingGoodsAsync(purchaseOrderId);
            if (po == null)
                return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Purchase Order not found.");

            if (po.Status != PurchaseOrderStatus.Approved)
                return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Approved POs can receive goods.");

            var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(warehouseId);
            if (warehouse == null)
                return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, $"Warehouse not found with ID: {warehouseId}.");

            try
            {
                var gr = new GoodsReceipt
                {
                    PurchaseOrderId = po.Id,
                    ReceivedById = dto.ReceivedById,
                    ReceivedDate = DateTime.UtcNow,
                    Notes = dto.Notes,
                };

                var productIds = dto.Lines.Select(l => l.ProductId).ToList();
                var products = await _unitOfWork.Repository<Product>().GetAllAsync(p => productIds.Contains(p.Id));

                var poLineDict = po.PurchaseOrderLines.ToDictionary(l => l.ProductId);

                foreach (var line in dto.Lines)
                {
                    if (!poLineDict.TryGetValue(line.ProductId, out var poLine))
                        return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                            $"Product Id: {line.ProductId} not in Purchase Order.");

                    var grLine = new GoodsReceiptLine
                    {
                        ProductId = line.ProductId,
                        ReceivedQuantity = line.ReceivedQuantity,
                        DamagedQuantity = line.DamagedQuantity,
                        GoodsReceipt = gr
                    };
                    gr.GoodsReceiptLines.Add(grLine);

                    var product = products.First(p => p.Id == line.ProductId);
                    product.TotalStock += line.ReceivedQuantity;
                    product.AvailableStock += line.ReceivedQuantity - line.DamagedQuantity;
                    _unitOfWork.Repository<Product>().Update(product);

                    var increaseDto = new IncreaseStockDTO
                    {
                        ProductId = line.ProductId,
                        WarehouseId = warehouseId,
                        DamagedQuantity = line.DamagedQuantity,
                        Quantity = line.ReceivedQuantity,
                        Reason = $"Goods Receipt for Purchase Order #{po.Id}"
                    };

                    await _stockService.IncreaseStockByIdAsync(increaseDto, IsCommit: false);
                }



                var totalOrdered = po.PurchaseOrderLines.Sum(l => l.Quantity);
                var totalReceived = po.GoodsReceipts.Sum(r => r.GoodsReceiptLines.Sum(l => l.ReceivedQuantity))
                                    + gr.GoodsReceiptLines.Sum(l => l.ReceivedQuantity);

                po.Status = totalReceived >= totalOrdered ? PurchaseOrderStatus.FullyReceived : PurchaseOrderStatus.PartiallyReceived;

                await _unitOfWork.Repository<GoodsReceipt>().AddNewAsync(gr);
                _unitOfWork.Repository<PurchaseOrder>().Update(po);

                await _unitOfWork.CommitAsync();

                var grDto = _mapper.Map<GetGoodsReceiptDTO>(gr);
                return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Goods received, stock updated, and PO status updated.", grDto);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetGoodsReceiptDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
