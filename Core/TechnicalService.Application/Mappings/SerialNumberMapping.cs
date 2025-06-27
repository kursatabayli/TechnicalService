using AutoMapper;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Commands;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;

namespace TechnicalService.Application.Mappings
{
    public class SerialNumberMapping : Profile
    {
        public SerialNumberMapping()
        {
            CreateMap<SerialNumber, SerialNumberResult>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Product.Brand.BrandName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Product.ProductType.Type));

            CreateMap<SerialNumberResult, SerialNumberDto>();

            CreateMap<CreateSerialNumberDto, CreateSerialNumberCommand>()
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => 
                src.RegisterDate.HasValue ? DateOnly.FromDateTime(src.RegisterDate.Value) : DateOnly.FromDateTime(DateTime.Now)));


            CreateMap<CreateSerialNumberCommand, SerialNumber>();
            CreateMap<UpdateSerialNumberDto, UpdateSerialNumberCommand>()
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src =>
                src.RegisterDate.HasValue ? DateOnly.FromDateTime(src.RegisterDate.Value) : DateOnly.FromDateTime(DateTime.Now)));
            CreateMap<UpdateSerialNumberCommand, SerialNumber>();
        }
    }
}
