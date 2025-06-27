using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.PersonnelAuth.Handlers
{
    public class PersonnelLoginHandler : IRequestHandler<PersonnelLoginCommand, Result<LoginResponse>>
    {
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly IAuthService _authService;
        private readonly ILogger<PersonnelLoginHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public PersonnelLoginHandler(IRepository<Personnel, Guid> repository, IHashService hashService, IUnitOfWork unitOfWork, IAuthService authService, ILogger<PersonnelLoginHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _hashService = hashService;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<LoginResponse>> Handle(PersonnelLoginCommand request, CancellationToken cancellationToken)
        {
            var personnel = await _repository.GetFirstOrDefaultAsync(x => x.InternalEmail == request.Email);
            var action = _returnMessages[ReturnMessages.Action_Login];
            if (personnel == null)
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.Login_InvalidCredentials], StatusCode.InvalidCredentials, HttpStatusCode.Unauthorized);

            bool isPasswordValid = _hashService.VerifyItem(request.Password, personnel.PasswordHash, personnel.PasswordSalt);
            if (!isPasswordValid)
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.Login_InvalidCredentials], StatusCode.InvalidCredentials, HttpStatusCode.Unauthorized);

            if (personnel.PersonnelStatus == PersonnelStatus.Terminated || personnel.PersonnelStatus == PersonnelStatus.Suspended)
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.Login_UserNotActive], StatusCode.Unauthorized, HttpStatusCode.Unauthorized);

            try
            {
                var (accessToken, accessTokenExpiration)  = _authService.GenerateJwtTokenForPersonnel(personnel);
                if (request.RememberMe)
                {
                    var refreshToken = _authService.GenerateRefreshToken();
                    DateTime refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
                    personnel.RefreshToken = refreshToken;
                    personnel.RefreshTokenExpiry = refreshTokenExpiration;
                    var loginUserResponse = new LoginResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiration = accessTokenExpiration,
                        RefreshTokenExpiration = refreshTokenExpiration
                    };
                    await _repository.UpdateAsync(personnel);
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
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action]);
                return Result<LoginResponse>.Failure(_returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
