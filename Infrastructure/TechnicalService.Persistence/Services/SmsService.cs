using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using TechnicalService.Application.Contracts.ServicesContracts;
using Twilio;
using TechnicalService.Persistence.Helpers;
using Microsoft.Extensions.Options;


namespace TechnicalService.Persistence.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSection _smsSection;

        public SmsService(IOptions<SmsSection> smsSection)
        {
            _smsSection = smsSection.Value;
        }

        public Task<bool> SendVerificationCode(string phoneNumber, int verificationCode)
        {
            var accountSid = _smsSection.AccountSid;
            var authToken = _smsSection.AuthToken;

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
            {
                From = new PhoneNumber(_smsSection.TwilioPhoneNumber),
                Body = $"Doğrulama kodunuz: {verificationCode}"
            };

            var message = MessageResource.Create(messageOptions);

            return Task.FromResult(message.Status == MessageResource.StatusEnum.Sent || message.Status == MessageResource.StatusEnum.Queued);
        }
    }
}
