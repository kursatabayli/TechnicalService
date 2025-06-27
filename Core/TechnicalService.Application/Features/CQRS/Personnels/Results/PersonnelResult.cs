using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.Personnels.Results
{
    public class PersonnelResult
    {
        public Guid Id { get; set; }
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
        public DateTime? CreatedAt { get; set; }
        public Role Role { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public string? ServiceName { get; set; }
        public int? TechnicalServiceId { get; set; }
        public DateOnly? TerminationDate { get; set; }
    }
}
