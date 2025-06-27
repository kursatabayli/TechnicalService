using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    public class UserLoginHandler : IRequestHandler<UserLoginCommand, Result<LoginResponse>>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly IAuthService _authService;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(IRepository<User, Guid> repository, IHashService hashService, IUnitOfWork unitOfWork, IAuthService authService, ILogger<UserLoginHandler> logger)
        {
            _repository = repository;
            _hashService = hashService;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetFirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                return Result<LoginResponse>.Failure("E-posta veya şifre hatalı", StatusCode.InvalidCredentials, HttpStatusCode.Unauthorized);

            bool isPasswordValid = _hashService.VerifyItem(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid)
                return Result<LoginResponse>.Failure("E-posta veya şifre hatalı", StatusCode.InvalidCredentials, HttpStatusCode.Unauthorized);

            if (!user.IsEmailConfirmed)
                return Result<LoginResponse>.Failure("E-posta adresiniz doğrulanmamış", StatusCode.EmailNotVerified, HttpStatusCode.Forbidden);

            try
            {
                var (accessToken, accessTokenExpiration) = _authService.GenerateJwtTokenForUser(user);
               
                if (request.RememberMe)
                {
                    var refreshToken = _authService.GenerateRefreshToken();
                    var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiry = refreshTokenExpiration;
                    var loginUserResponse = new LoginResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiration = accessTokenExpiration,
                        RefreshTokenExpiration = refreshTokenExpiration
                    };
                    await _repository.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                    return Result<LoginResponse>.Success(loginUserResponse);

                }
                else
                {
                    var loginUserResponse = new LoginResponse
                    {
                        AccessToken = accessToken,
                        AccessTokenExpiration = accessTokenExpiration
                    };
                    return Result<LoginResponse>.Success(loginUserResponse);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login sırasında beklenmedik hata oluştu.");
                return Result<LoginResponse>.Failure("Beklenmedik bir sunucu hatası oluştu.", StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
