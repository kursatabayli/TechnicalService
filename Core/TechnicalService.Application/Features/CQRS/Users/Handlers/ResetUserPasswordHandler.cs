using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Features.CQRS.Users.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using TechnicalService.DTOs.Enums;
using Microsoft.Extensions.Localization;
using TechnicalService.Application.Extensions;

namespace TechnicalService.Application.Features.CQRS.Users.Handlers
{
    public class ResetUserPasswordHandler : IRequestHandler<ResetUserPasswordCommand, Result>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IHashService _passwordHasher;
        private readonly ILogger<ResetUserPasswordHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public ResetUserPasswordHandler(IRepository<User, Guid> repository, IUnitOfWork unitOfWork, IAuthService authService, IHashService passwordHasher, ILogger<ResetUserPasswordHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _returnMessages = returnMessages;
        }
        public async Task<Result> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var pricipal = _authService.GetPrincipalFromVerifyToken(request.Token);
            var entityName = _returnMessages[ReturnMessages.EntityType_User];
            var action = _returnMessages[ReturnMessages.Action_PasswordReset];
            if (pricipal == null)
                return Result.Failure(_returnMessages[ReturnMessages.Error_User_TokenExpiredOrInvalid], StatusCode.TokenExpired, HttpStatusCode.Unauthorized);

            var userId = pricipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _repository.GetFirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
            if (user == null)
                return Result.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var (newHash, newsalt) = _passwordHasher.HashItem(request.NewPassword);
                user.PasswordHash = newHash;
                user.PasswordSalt = newsalt;
                await _repository.UpdateAsync(user);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return Result.Success("Şifre başarıyla sıfırlandı.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action]);
                return Result.Failure(_returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
