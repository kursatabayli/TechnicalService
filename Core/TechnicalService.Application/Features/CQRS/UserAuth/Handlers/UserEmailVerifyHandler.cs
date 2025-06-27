using MediatR;
using System.Security.Claims;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Enums;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using System.Net;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    internal class UserEmailVerifyHandler : IRequestHandler<UserEmailVerifyCommand, Result>
    {
        private readonly IRepository<EmailVerificationToken, Guid> _tokenRepository;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ILogger<UserEmailVerifyHandler> _logger;
        public UserEmailVerifyHandler(IRepository<EmailVerificationToken, Guid> tokenRepository, IAuthService authService, IUnitOfWork unitOfWork, IRepository<User, Guid> userRepository, ILogger<UserEmailVerifyHandler> logger)
        {
            _tokenRepository = tokenRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _logger = logger;
        }


        public async Task<Result> Handle(UserEmailVerifyCommand request, CancellationToken cancellationToken)
        {
            var principal = _authService.GetPrincipalFromVerifyToken(request.Token);

            if (principal == null)
                return Result.Failure("Doğrulama bağlantısının süresi sona ermiştir. Lütfen yeni doğrulama bağlantısı alınız.", StatusCode.TokenExpired, HttpStatusCode.Unauthorized);

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Result.Failure("Geçersiz token: Gerekli kullanıcı bilgisi bulunamadı.", StatusCode.InvalidToken, HttpStatusCode.BadRequest);

            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return Result.Failure("Belirtilen e-posta adresine sahip kullanıcı bulunamadı.", StatusCode.NotFound, HttpStatusCode.NotFound);

            var userToken = await _tokenRepository.GetFirstOrDefaultAsync(x => x.UserId == user.Id);

            if (user.IsEmailConfirmed)
                return Result.Failure("Bu e-posta adresi zaten doğrulanmış.",StatusCode.Conflict,HttpStatusCode.Conflict);

            try
            {
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(user);

                if (userToken != null)
                    await _tokenRepository.DeleteAsync(userToken);

                await _unitOfWork.SaveChangesWithTransactionAsync();

                return Result.Success("E-posta başarıyla doğrulandı!",HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "E-posta doğrulama sırasında hata oluştu. User ID: {UserId}", user?.Id);

                return Result.Failure("E-posta doğrulama işlemi sırasında beklenmedik bir sunucu hatası oluştu.",StatusCode.InternalServerError,HttpStatusCode.InternalServerError);
            }
        }
    }
}
