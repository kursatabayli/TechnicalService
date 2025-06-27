using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.PersonnelDTOs
{
    public class PersonnelMinimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public RoleDto Role { get; set; }
        public PersonnelStatusDto PersonnelStatus { get; set; }
        public string? ServiceName { get; set; }
        public int? TechnicalServiceId { get; set; }

    }
}
