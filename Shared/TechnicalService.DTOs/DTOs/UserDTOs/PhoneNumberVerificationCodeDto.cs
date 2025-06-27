namespace TechnicalService.DTOs.DTOs.UserDTOs
{
    public class PhoneNumberVerificationCodeDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int VerifyCode { get; set; }
    }
}
