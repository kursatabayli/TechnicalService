namespace TechnicalService.Domain.Entities
{
    public class EmailVerificationToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expiry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;
    }
}
