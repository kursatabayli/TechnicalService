namespace TechnicalService.Domain.Entities
{
    public class PhoneNumberVerificationCode
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public int VerifyCode { get; set; }
        public DateTime Expiry { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}
