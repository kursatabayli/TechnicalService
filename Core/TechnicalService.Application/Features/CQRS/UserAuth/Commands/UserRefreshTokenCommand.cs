using MediatR;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Commands
{
    public class UserRefreshTokenCommand : IRequest<Result<LoginResponse>>
    {
        public string RefreshToken { get; set; }

    }
}
