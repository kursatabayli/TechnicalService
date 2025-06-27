using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using TechnicalService.Persistence.Helpers;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Persistence.Helpers.Contracts;

namespace TechnicalService.Persistence.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ITemplateHelper _templateHelper;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ITemplateHelper templateHelper)
        {
            _smtpSettings = smtpSettings.Value;
            _templateHelper = templateHelper;
        }
        private async Task SendEmailAsync(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();
            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var replacements = new Dictionary<string, string>
            {
                { "PasswordResetLink", resetLink }
            };

            var templateContent = _templateHelper.GetTemplateContent("ResetPasswordEmail.html", replacements);

            await SendEmailAsync(email, "Şifre Sıfırlama", templateContent);
        }

        public async Task SendVerificationEmailAsync(string email, string verificationLink)
        {
            var replacements = new Dictionary<string, string>
            {
                { "VerificationLink", verificationLink }
            };

            var templateContent = _templateHelper.GetTemplateContent("VerificationEmail.html", replacements);

            await SendEmailAsync(email, "Hesabınızı Doğrulayın", templateContent);

        }

        public async Task SendPersonnelRegistrationEmailAsync(string email, string internalEmail, string password)
        {
            var replacements = new Dictionary<string, string>
            {
                { "InternalEmail", internalEmail },
                { "Password", password }
            };

            var templateContent = _templateHelper.GetTemplateContent("PersonnelRegistrationEmail.html", replacements);

            await SendEmailAsync(email, "Personel Kayıt Bilgileri", templateContent);
        }

        public async Task SendPersonnelNewPasswordEmailAsync(string email, string internalEmail, string password)
        {
            var replacements = new Dictionary<string, string>
            {
                { "InternalEmail", internalEmail },
                { "Password", password }
            };

            var templateContent = _templateHelper.GetTemplateContent("PersonnelNewPasswordEmail.html", replacements);

            await SendEmailAsync(email, "Yeni Şifre", templateContent);
        }

        public async Task SendServiceRecordCreateNotifyEmailAsync(string email, string trackingNumber, string serviceRecordId)
        {
            var replacements = new Dictionary<string, string>
            {
                { "TrackingNumber", trackingNumber },
                { "ServiceRecordId", serviceRecordId }
            };

            var templateContent = _templateHelper.GetTemplateContent("ServiceRecordCreateNotifyEmail.html", replacements);

            await SendEmailAsync(email, "Servis Kaydı Bilgilendirme", templateContent);
        }
    }
}
