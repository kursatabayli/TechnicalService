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
    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, Result>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IHashService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChangeUserPasswordHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public ChangeUserPasswordHandler(IRepository<User, Guid> repository, IHashService passwordHasher, IUnitOfWork unitOfWork, ILogger<ChangeUserPasswordHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_User];
            var action = _returnMessages[ReturnMessages.Action_Password];
            if (user == null)
                return Result.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            if (!_passwordHasher.VerifyItem(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return Result.Failure(_returnMessages[ReturnMessages.Error_InvalidCurrentPassword], StatusCode.InvalidCredentials, HttpStatusCode.BadRequest);

            try
            {
                var (newHash, newsalt) = _passwordHasher.HashItem(request.NewPassword);
                user.PasswordHash = newHash;
                user.PasswordSalt = newsalt;
                await _repository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success(_returnMessages[ReturnMessages.Message_Success_Updated, action], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, action], request.Id);
                return Result.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, action, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
