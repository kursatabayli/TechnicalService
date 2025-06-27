using MediatR;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Commands
{
    public class SendPhoneNumberVerificationCodeCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }
}
