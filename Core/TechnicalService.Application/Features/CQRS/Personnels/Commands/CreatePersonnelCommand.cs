using MediatR;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Commands
{
    public class CreatePersonnelCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateOnly? HireDate { get; set; }
        public Role Role { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public int? TechnicalServiceId { get; set; }
    }
}
