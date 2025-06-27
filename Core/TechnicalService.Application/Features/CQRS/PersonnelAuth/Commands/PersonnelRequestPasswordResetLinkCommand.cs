using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands
{
    public class PersonnelRequestPasswordResetLinkCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string IdentityNumber { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
