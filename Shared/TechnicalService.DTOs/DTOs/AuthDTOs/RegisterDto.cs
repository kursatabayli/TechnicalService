using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.AuthDTOs
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ApplyPassword { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderDto? Gender { get; set; }
    }
}
