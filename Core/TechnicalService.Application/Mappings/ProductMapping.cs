using AutoMapper;
using TechnicalService.Application.Features.CQRS.Products.Commands;
using TechnicalService.Application.Features.CQRS.Products.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.ProductDTOs;

namespace TechnicalService.Application.Mappings
{
    public class ProductMapping : Profile
    {
        public ProductMapping() 
        {

            CreateMap<Product, ProductResult>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProductType.Type));
            CreateMap<ProductResult, ProductDto>();
            CreateMap<CreateProductDto, CreateProductCommand>();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<ProductResult, UpdateProductDto>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<UpdateProductDto, UpdateProductCommand>();
        }
    }
}
