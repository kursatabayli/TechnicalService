namespace TechnicalService.DTOs.DTOs.UserDTOs
{
    public class ResetUserPasswordDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
