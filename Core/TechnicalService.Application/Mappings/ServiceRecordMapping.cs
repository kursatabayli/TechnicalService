using AutoMapper;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Commands;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;

namespace TechnicalService.Application.Mappings
{
    public class ServiceRecordMapping : Profile
    {
        public ServiceRecordMapping()
        {
            CreateMap<ServiceRecord, ServiceRecordListResult>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.UserProduct.SerialNumber.Serial_Number))
                .ForMember(dest => dest.ProductDetail, opt => opt.MapFrom(src => $"{src.UserProduct.SerialNumber.Product.Brand.BrandName} - {src.UserProduct.SerialNumber.Product.ProductName}"));

            CreateMap<ServiceRecord, ServiceRecordResult>()
                .ForMember(dest => dest.PersonnelName, opt => opt.MapFrom(src => src.Personnel != null ? $"{src.Personnel.Name} {src.Personnel.Surname}" : null));

            CreateMap<ServiceRecordResult, ServiceRecordDto>();


            CreateMap<ServiceRecord, UserServiceRecordResult>()
                .ForMember(dest => dest.Serial_Number, opt => opt.MapFrom(src => src.UserProduct.SerialNumber.Serial_Number))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.UserProduct.SerialNumber.Product.ProductName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.UserProduct.SerialNumber.Product.Brand.BrandName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.UserProduct.SerialNumber.Product.ProductType.Type))
                .ForMember(dest => dest.WarrantyDate, opt => opt.MapFrom(src => src.UserProduct.WarrantyDate));

            CreateMap<UserServiceRecordResult, UserServiceRecordsDto>();

            CreateMap<CreateServiceRecordDto, CreateServiceRecordCommand>();

            CreateMap<CreateServiceRecordCommand, ServiceRecord>();

            CreateMap<UpdateServiceRecordDto, UpdateServiceRecordCommand>();
            CreateMap<UpdateServiceRecordCommand, ServiceRecord>();
        }
    }
}
