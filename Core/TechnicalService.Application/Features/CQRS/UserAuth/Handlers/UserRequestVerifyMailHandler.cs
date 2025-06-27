using MediatR;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using System.Net;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    public class UserRequestVerifyMailHandler : IRequestHandler<UserRequestVerifyMailCommand, Result>
    {
        private readonly IRepository<EmailVerificationToken, Guid> _tokenRepository;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;
        private readonly ILogger<UserRequestVerifyMailHandler> _logger;
        public UserRequestVerifyMailHandler(IUnitOfWork unitOfWork, IEmailService emailService, IAuthService authService, IRepository<EmailVerificationToken, Guid> tokenRepository, IRepository<User, Guid> userRepository, ILogger<UserRequestVerifyMailHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _authService = authService;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<Result> Handle(UserRequestVerifyMailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                return Result.Failure(
                                "Bu e-posta adresine sahip bir kullanıcı bulunamadı.",
                                StatusCode.NotFound,
                                HttpStatusCode.NotFound);

            var existingToken = await _tokenRepository.GetFirstOrDefaultAsync(x => x.UserId == user.Id);

            // Eğer token varsa ve son 5 dakika içinde gönderilmişse
            if (existingToken != null && !string.IsNullOrEmpty(existingToken.Token) && existingToken.CreatedAt > DateTime.UtcNow.AddMinutes(-5))
                return Result.Failure(
                                 "Yeni bir doğrulama e-postası istemek için lütfen biraz bekleyin.",
                                 StatusCode.TooManyRequests,
                                 HttpStatusCode.TooManyRequests);
            try
            {
                var newToken = _authService.GenerateEmailVerificationToken(request.Email);

                if (existingToken == null)
                {
                    var emailToken = new EmailVerificationToken
                    {
                        Token = newToken,
                        Expiry = DateTime.UtcNow.AddHours(1),
                        UserId = user.Id
                    };
                    await _tokenRepository.CreateAsync(emailToken);
                }
                else
                {
                    existingToken.Token = newToken;
                    existingToken.Expiry = DateTime.UtcNow.AddHours(1);
                    await _tokenRepository.UpdateAsync(existingToken);
                }

                await _unitOfWork.SaveChangesAsync();
                await _emailService.SendVerificationEmailAsync(user.Email, newToken);

                return Result.Success(
                                "Doğrulama bağlantısı e-posta adresinize gönderildi.",
                                HttpStatusCode.OK);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Doğrulama e-postası isteği sırasında hata oluştu. Email: {Email}", request.Email);

                return Result.Failure(
                    "Doğrulama e-postası gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                    StatusCode.InternalServerError,
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}
