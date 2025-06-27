using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.PersonnelDTOs
{
    public class PersonnelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public GenderDto? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateOnly? HireDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public RoleDto Role { get; set; }
        public PersonnelStatusDto PersonnelStatus { get; set; }
        public string ServiceName { get; set; }
        public DateOnly? TerminationDate { get; set; }

    }
}
