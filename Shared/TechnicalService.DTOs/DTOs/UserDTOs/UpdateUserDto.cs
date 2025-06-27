using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public GenderDto? Gender { get; set; }
    }
}
