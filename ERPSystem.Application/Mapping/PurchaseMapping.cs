using AutoMapper;
using ERPSystem.Application.DTOs.GoodsReceipt;
using ERPSystem.Application.DTOs.PurchaseOrder;
using ERPSystem.Application.DTOs.Supplier;
using ERPSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class PurchaseMapping : Profile
    {
        public PurchaseMapping()
        {
            /* Supplier */
            CreateMap<Supplier, GetSupplierDTO>();
            CreateMap<SupplierDTO, Supplier>();


            /* Purchase Order */
            CreateMap<PurchaseOrder, GetPurchaseOrderDTO>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<PurchaseOrderDTO, PurchaseOrder>()
                .ForMember(dest => dest.PurchaseOrderLines, opt => opt.MapFrom(src => src.Lines));

            CreateMap<PurchaseOrderLineDto, PurchaseOrderLine>()
                .ForMember(dest => dest.LineTotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

            /* Goods Receipt */
            CreateMap<ReceiveGoodsDTO, GoodsReceipt>();
            CreateMap<GoodsReceipt, GetGoodsReceiptDTO>();
        }
    }
}
