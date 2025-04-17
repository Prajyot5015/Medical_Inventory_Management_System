﻿using AutoMapper;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Brand, CreateBrandDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Brand, UpdateBrandDTO>().ReverseMap();

            CreateMap<Manufacturer, CreateManufacturersDTO>().ReverseMap();
            CreateMap<Manufacturer, ManufacturersDTO>().ReverseMap();
            CreateMap<Manufacturer, UpdateManuFacturerDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CreateProductDTO, Product>().ReverseMap();
            CreateMap<UpdateProductDTO, Product>().ReverseMap();

            CreateMap<PurchaseOrder, PurchaseOrderDTO>().ReverseMap();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDTO>().ReverseMap();

            CreateMap<CreatePurchaseOrderDTO, PurchaseOrder>();
            CreateMap<PurchaseOrderItemDTO, PurchaseOrderItem>();

            CreateMap<PurchaseOrder, PurchaseOrderDTO>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDTO>();

            CreateMap<PurchaseOrderItem, PurchaseOrderItemResponseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<CreateSaleDto, Sale>();
            CreateMap<SaleItemDto, SalesItem>();

            CreateMap<Sale, SaleResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SalesItem, SaleItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Stock, StockDto>()
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Stock, StockDto>()
           .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
           .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.CurrentStock < src.LowStockThreshold))
           .ForMember(dest => dest.IsNearExpiry, opt => opt.MapFrom(src =>
               src.ExpiryDate.HasValue && src.ExpiryDate.Value <= DateTime.UtcNow.AddDays(7)
           ));

            CreateMap<Stock, StockDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.CurrentStock < src.LowStockThreshold))
                .ForMember(dest => dest.IsNearExpiry, opt => opt.MapFrom(src =>
                    src.ExpiryDate.HasValue && src.ExpiryDate.Value <= DateTime.UtcNow.AddDays(7)
                ));
        }
    }
}
