using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Commands
{
    public class ResetUserPasswordCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
