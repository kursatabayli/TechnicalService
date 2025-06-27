using MediatR;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands
{
    public class PersonnelRefreshTokenCommand : IRequest<Result<LoginResponse>>
    {
        public string RefreshToken { get; set; }

    }
}
