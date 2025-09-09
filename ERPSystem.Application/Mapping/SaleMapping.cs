using AutoMapper;
using ERPSystem.Application.DTOs.Customer;
using ERPSystem.Application.DTOs.Invoice;
using ERPSystem.Application.DTOs.Payment;
using ERPSystem.Application.DTOs.PurchaseOrder;
using ERPSystem.Application.DTOs.SalesOrder;
using ERPSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class SaleMapping : Profile
    {
        public SaleMapping()
        {
            CreateMap<Customer, GetCustomerDTO>();
            CreateMap<CustomerDTO, Customer>(); 

            CreateMap<SalesOrder, GetSalesOrderDTO>();
            CreateMap<SalesOrderDTO, SalesOrder>()
                .ForMember(dest => dest.SalesOrderLines, opt => opt.MapFrom(src => src.Lines));
            CreateMap<SalesOrderLineDTO, SalesOrderLine>();

            CreateMap<Invoice, GetInvoiceDTO>();
            CreateMap<InvoiceDTO, Invoice>();

            CreateMap<Payment, GetPaymentDTO>();
            CreateMap<PaymentDTO, Payment>();
        }
    }
}
