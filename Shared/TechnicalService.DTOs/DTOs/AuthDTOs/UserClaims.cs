namespace TechnicalService.DTOs.DTOs.AuthDTOs
{
    public class UserClaims
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
