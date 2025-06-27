using AutoMapper;
using TechnicalService.Application.Features.CQRS.ProductTypes.Commands;
using TechnicalService.Application.Features.CQRS.ProductTypes.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;

namespace TechnicalService.Application.Mappings
{
    public class ProductTypeMapping : Profile
    {
        public ProductTypeMapping()
        {

            CreateMap<ProductType, ProductTypeResult>();
            CreateMap<ProductTypeResult, ProductTypeDto>();
            CreateMap<CreateProductTypeDto, CreateProductTypeCommand>();
            CreateMap<CreateProductTypeCommand, ProductType>();
            CreateMap<ProductTypeResult, UpdateProductTypeCommand>();
            CreateMap<UpdateProductTypeDto, UpdateProductTypeCommand>();
            CreateMap<UpdateProductTypeCommand, ProductType>();
        }
    }
}
