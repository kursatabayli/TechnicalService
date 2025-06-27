using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Response;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.DTOs.Enums;
using Microsoft.Extensions.Localization;
using TechnicalService.Application.Extensions;

namespace TechnicalService.Application.Features.CQRS.PersonnelAuth.Handlers
{
    public class PersonnelRefreshTokenHandler : IRequestHandler<PersonnelRefreshTokenCommand, Result<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonnelRefreshTokenHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public PersonnelRefreshTokenHandler(IAuthService authService, IRepository<Personnel, Guid> repository, IUnitOfWork unitOfWork, ILogger<PersonnelRefreshTokenHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _authService = authService;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<LoginResponse>> Handle(PersonnelRefreshTokenCommand request, CancellationToken ct)
        {
            var personnel = await _repository.GetFirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);
            var action = _returnMessages[ReturnMessages.Action_RefreshToken];
            if (personnel == null)
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.RefreshToken_InvalidOrExpired], StatusCode.NotFound, HttpStatusCode.NotFound);

            if (request.RefreshToken != personnel.RefreshToken)
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.RefreshToken_VerificationFailed], StatusCode.ValidationError, HttpStatusCode.Unauthorized);
            try
            {
                var (newAccessToken, tokenExpiration) = _authService.GenerateJwtTokenForPersonnel(personnel);
                var newRefreshToken = _authService.GenerateRefreshToken();
                DateTime refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                personnel.RefreshToken = newRefreshToken;
                personnel.RefreshTokenExpiry = refreshTokenExpiration;
                await _repository.UpdateAsync(personnel);
                await _unitOfWork.SaveChangesAsync();
                var loginUserResponse = new LoginResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    AccessTokenExpiration = tokenExpiration,
                    RefreshTokenExpiration = refreshTokenExpiration
                };
                return Result<LoginResponse>.Success(loginUserResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action]);
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.RefreshToken_UnexpectedError], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
