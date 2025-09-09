using AutoMapper;
using ERPSystem.Application.DTOs.Category;
using ERPSystem.Application.DTOs.Product;
using ERPSystem.Application.DTOs.Stock;
using ERPSystem.Application.DTOs.Warehouse;
using ERPSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class InventoryMapping : Profile
    {
        public InventoryMapping()
        {
            /* Products Mappings */
            CreateMap<Product, GetProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ProductDTO, Product>();


            /* Category Mappings */
            CreateMap<Category, GetCategoryDTO>();

            CreateMap<CategoryDTO, Category>();


            /* Warehouses Mappings */
            CreateMap<Warehouse, GetWarehousesDTO>();

            CreateMap<WarehouseDTO, Warehouse>();


            /* Stocks Mappings */
            CreateMap<Stock, GetStockByWarehouseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Stock, GetStockDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<AddStockDTO, Stock>();

            /* StockMovements Mappings */

            CreateMap<DecreaseStockDTO, StockMovement>()
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.StockId, opt => opt.Ignore());


        }
    }
}
