using AutoMapper;
using TechnicalService.Application.Features.CQRS.Users.Commands;
using TechnicalService.Application.Features.CQRS.Users.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserResult>();
            CreateMap<UserResult, UserDto>();
            CreateMap<UpdateUserDto, UpdateUserCommand>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src =>
                src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender ?? GenderDto.Empty));


            CreateMap<UpdateUserCommand, User>();
            CreateMap<ChangeUserPasswordDto, ChangeUserPasswordCommand>();
            CreateMap<ResetUserPasswordDto, ResetUserPasswordCommand>();
        }
    }
}
