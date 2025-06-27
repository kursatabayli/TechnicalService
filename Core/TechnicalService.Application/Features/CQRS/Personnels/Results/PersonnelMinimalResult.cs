using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.Personnels.Results
{
    public class PersonnelMinimalResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public string? ServiceName { get; set; }
        public int? TechnicalServiceId { get; set; }
    }
}
