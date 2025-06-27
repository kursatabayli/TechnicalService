using System.ComponentModel.DataAnnotations;
using TechnicalService.Domain.Enums;

namespace TechnicalService.Domain.Entities
{
    public class Personnel
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
        public Gender? Gender { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateOnly? HireDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public Role Role { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public TechnicalService? TechnicalServices { get; set; }
        public int TechnicalServiceId { get; set; }
        public DateOnly? TerminationDate { get; set; }
        [MaxLength(128)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public List<ServiceRecord>? ServiceRecords { get; set; }
        public List<ServiceRecordStep>? ServiceRecordSteps { get; set; }

    }
}
