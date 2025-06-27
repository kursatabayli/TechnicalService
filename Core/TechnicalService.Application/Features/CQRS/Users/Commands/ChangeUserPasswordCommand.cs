using MediatR;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Commands
{
    public class ChangeUserPasswordCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
