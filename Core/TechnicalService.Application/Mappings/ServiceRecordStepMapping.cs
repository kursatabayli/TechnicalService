using AutoMapper;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;

namespace TechnicalService.Application.Mappings
{
    public class ServiceRecordStepMapping : Profile
    {
        public ServiceRecordStepMapping()
        {
            CreateMap<ServiceRecordStep, ServiceRecordStepResult>()
                .ForMember(dest => dest.PersonnelFullName, opt => opt.MapFrom(src => src.Personnel != null ? $"{src.Personnel.Name} {src.Personnel.Surname}" : null));

            CreateMap<ServiceRecordStepResult, ServiceRecordStepDto>();

            CreateMap<AddServiceRecordStepDto, AddServiceRecordStepCommand>();
            CreateMap<AddServiceRecordStepCommand, ServiceRecordStep>();

            CreateMap<UpdateServiceRecordStepDto, UpdateServiceRecordStepCommand>();
            CreateMap<UpdateServiceRecordStepCommand, ServiceRecordStep>();
        }
    }
}
