using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Commands
{
    public class UserEmailVerifyCommand : IRequest<Result>
    {
        public string Token { get; set; }
    }
}
