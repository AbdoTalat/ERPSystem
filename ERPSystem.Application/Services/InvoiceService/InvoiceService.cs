using AutoMapper;
using ERPSystem.Application.DTOs.Invoice;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain;
using Helper.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using ERPSystem.Domain.Enums;

namespace ERPSystem.Application.Services.InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetInvoiceDTO>>> GetAllInvoicesAsync()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsDtoAsync<GetInvoiceDTO>();

            return ApiResponseHelper<IEnumerable<GetInvoiceDTO>>.ResponseSuccess(data: invoices);

        }
        public async Task<ApiResponseHelper<GetInvoiceDTO>> GetInvoiceByIdAsync(int Id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsDtoAsync<GetInvoiceDTO>(Id);

            if (invoice == null)
            {
                return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Invoice Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetInvoiceDTO>.ResponseSuccess(data: invoice);
        }
        public async Task<ApiResponseHelper<IEnumerable<GetInvoiceDTO>>> GetInvoicesBySalesOrderIdAsync(int SalesOrderId)
        {
            var invoices = await _unitOfWork.Repository<Invoice>()
                .GetAllAsDtoAsync<GetInvoiceDTO>(e => e.SalesOrderId == SalesOrderId);

            return ApiResponseHelper<IEnumerable<GetInvoiceDTO>>.ResponseSuccess(data: invoices);
        }
        public async Task<ApiResponseHelper<GetInvoiceDTO>> AddInvoiceAsync(int SalesOrderId)
        {
            try
            {
                var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(SalesOrderId);
                if (salesOrder == null)
                {
                    return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Sales Order not found.");
                }
                if(salesOrder.Status != SalesOrderStatus.Completed && salesOrder.Status != SalesOrderStatus.Confirmed)
                {
                    return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Invoice can only be created for Confirmed or Completed Sales Orders.");
                }
                var invoiceToAdd = new Invoice
                {
                    InvoiceDate = DateTime.UtcNow,
                    TotalAmount = salesOrder.TotalAmount,
                    SalesOrderId = SalesOrderId,
                    Status = InvoiceStatus.Unpaid
                };
                await _unitOfWork.Repository<Invoice>().AddNewAsync(invoiceToAdd);
                await _unitOfWork.CommitAsync();

                var addedInvoice = await _unitOfWork.Repository<Invoice>().GetByIdAsDtoAsync<GetInvoiceDTO>(invoiceToAdd.Id);

                return ApiResponseHelper<GetInvoiceDTO>.ResponseSuccess(StatusCodes.CREATED,
                    "Invoice Created Successfully", addedInvoice);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetInvoiceDTO>> CancellInvoiceByIdAsync(int Id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(Id);
            if (invoice == null)
            {
                return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Invoice Not found with ID: {Id}");
            }
            if (invoice.Status != InvoiceStatus.Unpaid)
            {
                return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    $"Invoice cannot be Cancelled because it is {invoice.Status}. Only Unpaid invoices can be cancelled.");
            }

            try
            {
                invoice.Status = InvoiceStatus.Cancelled;
                _unitOfWork.Repository<Invoice>().Update(invoice);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<GetInvoiceDTO>.ResponseSuccess(message: "Invoice Cancelled Successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetInvoiceDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> DeleteInvoiceAsync(int Id)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(Id);
            if (invoice == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Invoice Not found with ID: {Id}");
            }
            if (invoice.Status != InvoiceStatus.Unpaid)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.BAD_REQUEST,
                    $"Invoice cannot be deleted because it is {invoice.Status}. Only Unpaid invoices can be deleted.");
            }
            try
            {
                _unitOfWork.Repository<Invoice>().Delete(invoice);
                await _unitOfWork.CommitAsync();

                return ApiResponseHelper<object>.ResponseSuccess(message: "Invoice Deleted Successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.CONFLICT,
                    "Cannot delete Invoice because it is referenced by another record."
                );
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
