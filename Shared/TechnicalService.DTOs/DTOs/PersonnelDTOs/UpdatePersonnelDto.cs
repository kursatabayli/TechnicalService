using System.ComponentModel.DataAnnotations;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.PersonnelDTOs
{
    public class UpdatePersonnelDto
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Surname { get; set; }
        [MaxLength(11)]
        public string IdentityNumber { get; set; }
        [MaxLength(40)]
        public string Email { get; set; }
        [MaxLength(40)]
        public string InternalEmail { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public GenderDto? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public RoleDto? Role { get; set; }
        public PersonnelStatusDto? PersonnelStatus { get; set; }
        public TechnicalServiceDto TechnicalServices { get; set; }
        public int? TechnicalServiceId { get; set; }
    }
}
