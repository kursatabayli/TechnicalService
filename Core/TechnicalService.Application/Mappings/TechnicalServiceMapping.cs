using AutoMapper;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Commands;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Results;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;

namespace TechnicalService.Application.Mappings
{
    public class TechnicalServiceMapping : Profile
    {
        public TechnicalServiceMapping()
        {
            CreateMap<Domain.Entities.TechnicalService, TechnicalServiceResult>();
            CreateMap<TechnicalServiceResult, TechnicalServiceDto>();

            CreateMap<CreateTechnicalServiceDto, CreateTechnicalServiceCommand>();
            CreateMap<CreateTechnicalServiceCommand, Domain.Entities.TechnicalService>();

            CreateMap<TechnicalServiceResult, UpdateTechnicalServiceCommand>();
            CreateMap<UpdateTechnicalServiceDto, UpdateTechnicalServiceCommand>();
            CreateMap<UpdateTechnicalServiceCommand, Domain.Entities.TechnicalService>();
        }
    }
}
