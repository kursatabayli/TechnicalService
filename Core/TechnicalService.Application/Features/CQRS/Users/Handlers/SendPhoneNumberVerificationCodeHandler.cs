using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Users.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Handlers
{
    public class SendPhoneNumberVerificationCodeHandler : IRequestHandler<SendPhoneNumberVerificationCodeCommand, Result>
    {
        private readonly IRepository<PhoneNumberVerificationCode, int> _repository;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        private readonly ILogger<SendPhoneNumberVerificationCodeHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public SendPhoneNumberVerificationCodeHandler(IRepository<PhoneNumberVerificationCode, int> repository, IUnitOfWork unitOfWork, ISmsService smsService, ILogger<SendPhoneNumberVerificationCodeHandler> logger, IRepository<User, Guid> userRepository, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _logger = logger;
            _userRepository = userRepository;
            _returnMessages = returnMessages;
        }

        public async Task<Result> Handle(SendPhoneNumberVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_User];
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    return Result.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

                var isCodeExist = await _repository.GetFirstOrDefaultAsync(x => x.UserId == request.UserId);
                if (isCodeExist != null && isCodeExist.Expiry > DateTime.UtcNow)
                    return Result.Failure("Yeni bir doğrulama kodu almadan önce bir süre bekleyin.", StatusCode.TooManyRequests, HttpStatusCode.TooManyRequests);

                 

                var verificationCode = new Random().Next(100000, 999999);

                var smsResponse = await _smsService.SendVerificationCode(user.PhoneNumber, verificationCode);

                if (!smsResponse)
                    return Result.Failure("Doğrulama kodu gönderilirken bir hata oluştu (SMS Servisi).", StatusCode.ExternalServiceError, HttpStatusCode.InternalServerError);
                else
                {
                    var phoneNumberVerificationCode = new PhoneNumberVerificationCode
                    {
                        UserId = request.UserId,
                        VerifyCode = verificationCode,
                    };

                    if (isCodeExist != null)
                        await _repository.DeleteAsync(isCodeExist);

                    await _repository.CreateAsync(phoneNumberVerificationCode);
                    await _unitOfWork.SaveChangesWithTransactionAsync();
                    return Result.Success("Doğrulama kodu başarıyla gönderildi.", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doğrulama kodu gönderirken bir hata meydana geldi.");
                return Result.Failure("Doğrulama kodu gönderilirken sunucu tarafında bir hata oluştu.", StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
