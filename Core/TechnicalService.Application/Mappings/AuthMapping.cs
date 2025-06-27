using AutoMapper;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Mappings
{
    public class AuthMapping : Profile
    {
        public AuthMapping()
        {
            CreateMap<LoginDto, UserLoginCommand>();

            CreateMap<RegisterDto, UserRegisterCommand>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender ?? GenderDto.Empty));

            CreateMap<UserRegisterCommand, User>();
            CreateMap<RefreshTokenDto, UserRefreshTokenCommand>();


            CreateMap<LoginDto, PersonnelLoginCommand>();
            CreateMap<RefreshTokenDto, PersonnelRefreshTokenCommand>();

        }
    }
}
