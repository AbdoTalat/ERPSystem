using AutoMapper;
using ERPSystem.Application.DTOs.PurchaseOrder;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using Helper.API;
using Helper.Constants;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Application.IRepository;
using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.DTOs.Product;

namespace ERPSystem.Application.Services.PurchaseOrderService
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>> GetAllPurchaseOrderAsync()
        {
            var orders = await _unitOfWork.Repository<PurchaseOrder>().GetAllAsDtoAsync<GetPurchaseOrderDTO>();

            return ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>.ResponseSuccess(data: orders);
        }
        public async Task<ApiResponseHelper<GetPurchaseOrderDTO>> GetPurchaseOrderByIdAsync(int Id)
        {
            var po = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsDtoAsync<GetPurchaseOrderDTO>(Id);
            if (po != null)
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseSuccess(data:po);
            }
            return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Purchase Orcer not found with ID: {Id}");
        }
        public async Task<ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>> GetPurchaseOrdersBySupplierIdAsync(int SupplierId)
        {
            var po = await _unitOfWork.Repository<PurchaseOrder>()
                .GetAllAsDtoAsync<GetPurchaseOrderDTO>(po => po.SupplierId == SupplierId);

            return ApiResponseHelper<IEnumerable<GetPurchaseOrderDTO>>.ResponseSuccess(data: po);
        }
        public async Task<ApiResponseHelper<GetPurchaseOrderDTO>> AddPurchaseOrderAsync(PurchaseOrderDTO dto)
        {
            if(!dto.Lines.Any())
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Order must have at least one line.");
            }

            var productIDs = dto.Lines.Select(l => l.ProductId).ToList();
            var invalidProductIds = await _productRepository.CheckProductsExistById(productIDs);

            if (invalidProductIds.Any())
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    $"Invalid Product IDs: {string.Join(", ", invalidProductIds)}");
            }
            try
            {
                var OrderToAdd = _mapper.Map<PurchaseOrder>(dto);
                OrderToAdd.Status = PurchaseOrderStatus.Pending;

                OrderToAdd.TotalAmount = OrderToAdd.PurchaseOrderLines.Sum(l => l.LineTotal);

                await _unitOfWork.Repository<PurchaseOrder>().AddNewAsync(OrderToAdd);
                await _unitOfWork.CommitAsync();

                var AddedOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsDtoAsync<GetPurchaseOrderDTO>(OrderToAdd.Id);

                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseSuccess(StatusCodes.CREATED, "New Order created successfully.", AddedOrder);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetPurchaseOrderDTO>> EditPurchaseOrderByIdAsync(int Id, PurchaseOrderDTO dto)
        {
            var oldPurchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(Id);
            if (oldPurchaseOrder == null)
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, message: $"Purchase Order Not found with ID: {Id}");
            }
            if (!dto.Lines.Any())
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Order must have at least one line.");
            }

            var productIDs = dto.Lines.Select(l => l.ProductId).ToList();
            var invalidProductIds = await _productRepository.CheckProductsExistById(productIDs);

            if (invalidProductIds.Any())
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    $"Invalid Product IDs: {string.Join(", ", invalidProductIds)}");
            }

            try
            {
                var poLines = await _unitOfWork.Repository<PurchaseOrderLine>().
                    GetAllAsync(pol => pol.PurchaseOrderId == oldPurchaseOrder.Id);

                _unitOfWork.Repository<PurchaseOrderLine>().DeleteRange(poLines);

                _mapper.Map(dto, oldPurchaseOrder);

                oldPurchaseOrder.TotalAmount = oldPurchaseOrder.PurchaseOrderLines.Sum(l => l.LineTotal);

                _unitOfWork.Repository<PurchaseOrder>().Update(oldPurchaseOrder);
                await _unitOfWork.CommitAsync();

                var updatedPO = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsDtoAsync<GetPurchaseOrderDTO>(Id);

                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseSuccess(message: "Purchase Order Updated Successfully.", data: updatedPO);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetPurchaseOrderDTO>> ApprovePurchaseOrderByIdAsync(int Id)
        {
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(Id);
            if (purchaseOrder == null)
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Purchase Order not found.");

            if (purchaseOrder.Status != PurchaseOrderStatus.Pending)
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Pending PurchaseOrders can be approved.");

            purchaseOrder.Status = PurchaseOrderStatus.Approved;
            _unitOfWork.Repository<PurchaseOrder>().Update(purchaseOrder);
            await _unitOfWork.CommitAsync();

            var dto = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsDtoAsync<GetPurchaseOrderDTO>(purchaseOrder.Id);

            return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseSuccess(StatusCodes.OK, "Purchase Order approved.", dto);
        }
        public async Task<ApiResponseHelper<GetPurchaseOrderDTO>> CancelPurchaseOrderByIdAsync(int Id)
        {
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(Id);
            if (purchaseOrder == null)
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Purchase Order not found.");

            if (purchaseOrder.Status != PurchaseOrderStatus.Pending && purchaseOrder.Status != PurchaseOrderStatus.Approved)
                return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Only Pending or Approved Purchase Orders can be Cancelled.");

            purchaseOrder.Status = PurchaseOrderStatus.Cancelled;
            _unitOfWork.Repository<PurchaseOrder>().Update(purchaseOrder);
            await _unitOfWork.CommitAsync();

            var dto = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsDtoAsync<GetPurchaseOrderDTO>(purchaseOrder.Id);

            return ApiResponseHelper<GetPurchaseOrderDTO>.ResponseSuccess(StatusCodes.OK, "Purchase Order approved.", dto);
        }
    }
}
