namespace TechnicalService.Application.Contracts.ServicesContracts
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationLink);
        Task SendPasswordResetEmailAsync(string email, string resetLink);
        Task SendPersonnelRegistrationEmailAsync(string email, string internalEmail, string password);
        Task SendPersonnelNewPasswordEmailAsync(string email, string internalEmail, string password);
        Task SendServiceRecordCreateNotifyEmailAsync(string email, string trackingNumber, string serviceRecordId);
    }
}
