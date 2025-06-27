using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Response;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    public class UserRefreshTokenHandler : IRequestHandler<UserRefreshTokenCommand, Result<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IRepository<User, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserRefreshTokenHandler> _logger;

        public UserRefreshTokenHandler(IAuthService authService, IRepository<User, Guid> repository, IUnitOfWork unitOfWork, ILogger<UserRefreshTokenHandler> logger)
        {
            _authService = authService;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(UserRefreshTokenCommand request, CancellationToken ct)
        {
            try
            {
                var user = await _repository.GetFirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);

                if (user == null)
                    return Result<LoginResponse>.Failure("Oturum Doğrulanamadı.", StatusCode.NotFound, HttpStatusCode.NotFound);


                var (newAccessToken, accessTokenExpiration) = _authService.GenerateJwtTokenForUser(user);
                var newRefreshToken = _authService.GenerateRefreshToken();
                DateTime refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = refreshTokenExpiration;
                await _repository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                var loginUserResponse = new LoginResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    AccessTokenExpiration = accessTokenExpiration,
                    RefreshTokenExpiration = refreshTokenExpiration
                };
                return Result<LoginResponse>.Success(loginUserResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oturum yenileme hatası. Kullanıcı ID: {UserId}", request);
                return Result<LoginResponse>.Failure("Beklenmedik bir sunucu hatası oluştu.", StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
