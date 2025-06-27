namespace TechnicalService.DTOs.DTOs.PersonnelDTOs
{
    public class PersonnelRequestPasswordResetLinkDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InternalEmail { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
