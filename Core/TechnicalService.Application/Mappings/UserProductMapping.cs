using AutoMapper;
using TechnicalService.Application.Features.CQRS.UserProducts.Commands;
using TechnicalService.Application.Features.CQRS.UserProducts.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.UserProductDTOs;

namespace TechnicalService.Application.Mappings
{
    public class UserProductMapping : Profile
    {
        public UserProductMapping()
        {
            CreateMap<UserProduct, UserProductResult>()
                .ForMember(dest => dest.Serial_Number, opt => opt.MapFrom(src => src.SerialNumber.Serial_Number))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.SerialNumber.Product.ProductName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.SerialNumber.Product.Brand.BrandName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.SerialNumber.Product.ProductType.Type));

            CreateMap<UserProductResult, UserProductDto>();

            CreateMap<AddUserProductDto, AddUserProductCommand>()
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.PurchaseDate)));

            CreateMap<AddUserProductCommand, UserProduct>()
                .ForMember(dest => dest.SerialNumber, opt => opt.Ignore());
            CreateMap<UpdateUserProductDto, UpdateUserProductCommand>();
            CreateMap<UpdateUserProductCommand, UserProduct>();
        }
    }
}
