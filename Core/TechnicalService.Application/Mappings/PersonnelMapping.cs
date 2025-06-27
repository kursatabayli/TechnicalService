using AutoMapper;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.Application.Features.CQRS.Personnels.Commands;
using TechnicalService.Application.Features.CQRS.Personnels.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;

namespace TechnicalService.Application.Mappings
{
    public class PersonnelMapping : Profile
    {
        public PersonnelMapping()
        {
            CreateMap<Personnel, PersonnelResult>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.TechnicalServices.ServiceName));
            CreateMap<PersonnelResult, PersonnelDto>();

            CreateMap<Personnel, PersonnelMinimalResult>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.TechnicalServices.ServiceName));
            CreateMap<PersonnelMinimalResult, PersonnelMinimalDto>();

            CreateMap<CreatePersonnelDto, CreatePersonnelCommand>()
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src =>
                    src.HireDate.HasValue ? DateOnly.FromDateTime(src.HireDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

            CreateMap<CreatePersonnelCommand, Personnel>();

            CreateMap<PersonnelRequestPasswordResetLinkDto, PersonnelRequestPasswordResetLinkCommand>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

            CreateMap<UpdatePersonnelDto, UpdatePersonnelCommand>()
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src =>
                    src.HireDate.HasValue ? DateOnly.FromDateTime(src.HireDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));
            CreateMap<UpdatePersonnelCommand, Personnel>();

            CreateMap<ChangePersonnelPasswordDto, ChangePersonnelPasswordCommand>();
        }
    }
}
