using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Commands
{
    public class UserRequestResetPasswordLinkCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
}
