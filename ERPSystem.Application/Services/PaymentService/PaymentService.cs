using AutoMapper;
using ERPSystem.Application.DTOs.Payment;
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

namespace ERPSystem.Application.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetPaymentDTO>>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsDtoAsync<GetPaymentDTO>();

            return ApiResponseHelper<IEnumerable<GetPaymentDTO>>.ResponseSuccess(data: payments);

        }
        public async Task<ApiResponseHelper<GetPaymentDTO>> GetPaymentByIdAsync(int Id)
        {
            var payment = await _unitOfWork.Repository<Payment>().GetByIdAsDtoAsync<GetPaymentDTO>(Id);

            if (payment == null)
            {
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Payment Not Found With ID: {Id}");
            }

            return ApiResponseHelper<GetPaymentDTO>.ResponseSuccess(data: payment);
        }
        public async Task<ApiResponseHelper<IEnumerable<GetPaymentDTO>>> GetPaymentsByInvoiceIdAsync(int InvoiceId)
        {
            var invoices = await _unitOfWork.Repository<Payment>()
                .GetAllAsDtoAsync<GetPaymentDTO>(e => e.InvoiceId == InvoiceId);

            return ApiResponseHelper<IEnumerable<GetPaymentDTO>>.ResponseSuccess(data: invoices);
        }
        public async Task<ApiResponseHelper<GetPaymentDTO>> AddPaymentAsync(int InvoiceId, PaymentDTO dto)
        {                
            var invoice = await _unitOfWork.Repository<Invoice>()
                .GetByIdAsync(InvoiceId, includes: i => i.Payments);
            if (invoice == null)    
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Invoice not found.");
                
            if (invoice.Status != InvoiceStatus.Unpaid && invoice.Status != InvoiceStatus.PartiallyPaid)    
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Payment can only be created for Unpaid or Partially Paid Invoices.");
            try
            {
                var alreadyPaid = invoice.Payments.Sum(p => p.Amount);
                var remaining = invoice.TotalAmount - alreadyPaid;

                if (remaining <= 0)
                    return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Payment is already fully paid.");
                
                if (dto.Amount <= 0)
                    return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Payment amount must be greater than 0.");
                
                if (dto.Amount > remaining)
                    return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, $"Payment exceeds remaining balance. Remaining: {remaining}");
                
                var paymentToAdd = new Payment
                {
                    PaymentDate = DateTime.UtcNow,
                    Amount = dto.Amount,
                    InvoiceId = invoice.Id,
                    Method = Enum.Parse<PaymentMethod>(dto.Method)
                };
                await _unitOfWork.Repository<Payment>().AddNewAsync(paymentToAdd);

                var newRemaining = remaining - dto.Amount;
                invoice.Status = newRemaining == 0 ? InvoiceStatus.Paid : InvoiceStatus.PartiallyPaid;
                _unitOfWork.Repository<Invoice>().Update(invoice);

                await _unitOfWork.CommitAsync();

                var addedPayment = await _unitOfWork.Repository<Payment>().GetByIdAsDtoAsync<GetPaymentDTO>(paymentToAdd.Id);

                return ApiResponseHelper<GetPaymentDTO>.ResponseSuccess(StatusCodes.CREATED,"Payment Created Successfully", addedPayment);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetPaymentDTO>> EditPaymentAsync(int paymentId, PaymentDTO dto)
        {
            var payment = await _unitOfWork.Repository<Payment>().GetByIdAsync(paymentId);
            if (payment == null)
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Payment not found with ID: {paymentId}");

            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(payment.InvoiceId);

            if (invoice == null)
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Invoice not found.");

            if (invoice.Status == InvoiceStatus.Paid || invoice.Status == InvoiceStatus.Cancelled)
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Cannot update payment for Paid or Cancelled invoices.");

            try
            {
                var alreadyPaid = invoice.Payments.Where(p => p.Id != paymentId).Sum(p => p.Amount);
                var remaining = invoice.TotalAmount - alreadyPaid;

                if (dto.Amount > remaining)
                {
                    return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(
                        StatusCodes.BAD_REQUEST, $"Updated payment exceeds remaining balance. Remaining: {remaining}");
                }

                payment.Amount = dto.Amount;
                payment.Method = Enum.Parse<PaymentMethod>(dto.Method);
                payment.PaymentDate = DateTime.UtcNow;

                _unitOfWork.Repository<Payment>().Update(payment);

                var totalPaid = invoice.Payments.Where(p => p.Id != paymentId).Sum(p => p.Amount) + dto.Amount;
                var newRemaining = invoice.TotalAmount - totalPaid;

                if (totalPaid == 0)
                    invoice.Status = InvoiceStatus.Unpaid;
                else if (totalPaid < invoice.TotalAmount)
                    invoice.Status = InvoiceStatus.PartiallyPaid;
                else if (totalPaid == invoice.TotalAmount)
                    invoice.Status = InvoiceStatus.Paid;

                _unitOfWork.Repository<Invoice>().Update(invoice);
                await _unitOfWork.CommitAsync();

                var updatedPayment = await _unitOfWork.Repository<Payment>().GetByIdAsDtoAsync<GetPaymentDTO>(payment.Id);

                return ApiResponseHelper<GetPaymentDTO>.ResponseSuccess(message: "Payment updated successfully.", data: updatedPayment);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetPaymentDTO>> RefundPaymentAsync(int paymentId, decimal refundAmount)
        {
            var payment = await _unitOfWork.Repository<Payment>().GetByIdAsync(paymentId);
            if (payment == null)
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Payment not found with ID: {paymentId}");
            
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(payment.InvoiceId);
            if (invoice == null)
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.NOT_FOUND, "Invoice not found.");
            
            try
            {
                if (refundAmount <= 0 || refundAmount > payment.Amount)
                {
                    return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(
                        StatusCodes.BAD_REQUEST, $"Refund amount must be between 0 and {payment.Amount}.");
                }

                var refundPayment = new Payment
                {
                    PaymentDate = DateTime.UtcNow,
                    Amount = -refundAmount,
                    Method = payment.Method, 
                    InvoiceId = invoice.Id,
                };                
                await _unitOfWork.Repository<Payment>().AddNewAsync(refundPayment);

                var totalPaid = invoice.Payments.Sum(p => p.Amount) + refundPayment.Amount;
                var newRemaining = invoice.TotalAmount - totalPaid;

                if (totalPaid == 0)
                    invoice.Status = InvoiceStatus.Unpaid;
                else if (totalPaid < invoice.TotalAmount)
                    invoice.Status = InvoiceStatus.PartiallyPaid;
                else if (totalPaid == invoice.TotalAmount)
                    invoice.Status = InvoiceStatus.Paid;

                _unitOfWork.Repository<Invoice>().Update(invoice);
                await _unitOfWork.CommitAsync();

                var addedRefund = await _unitOfWork.Repository<Payment>().GetByIdAsDtoAsync<GetPaymentDTO>(refundPayment.Id);

                return ApiResponseHelper<GetPaymentDTO>.ResponseSuccess(StatusCodes.CREATED, "Refund processed successfully.", addedRefund);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetPaymentDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

    }
}
