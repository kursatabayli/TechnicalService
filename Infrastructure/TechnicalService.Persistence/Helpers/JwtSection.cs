namespace TechnicalService.Persistence.Helpers
{
    public class JwtSection
    {
        public string Key { get; set; }
        public string RefreshTokenKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
