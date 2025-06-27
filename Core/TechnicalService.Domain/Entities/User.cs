using System.ComponentModel.DataAnnotations;
using TechnicalService.Domain.Enums;

namespace TechnicalService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Surname { get; set; }
        [MaxLength(40)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateOnly? BirthDate { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public Role Role { get; set; } = Role.User;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsPhoneNumberConfirmed { get; set; } = false;
        [MaxLength(128)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }


        public List<UserProduct>? ProductSerialNumbers { get; set; }
        public List<EmailVerificationToken>? EmailVerificationTokens { get; set; }
        public List<ServiceRecord>? ServiceRecords { get; set; }
    }
}
