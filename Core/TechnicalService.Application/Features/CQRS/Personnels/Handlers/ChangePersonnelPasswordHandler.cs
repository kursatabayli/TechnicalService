using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Personnels.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Handlers
{
    internal class ChangePersonnelPasswordHandler : IRequestHandler<ChangePersonnelPasswordCommand , Result>
    {
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly IHashService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChangePersonnelPasswordHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public ChangePersonnelPasswordHandler(IRepository<Personnel, Guid> repository, IHashService passwordHasher, IUnitOfWork unitOfWork, ILogger<ChangePersonnelPasswordHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result> Handle(ChangePersonnelPasswordCommand request, CancellationToken cancellationToken)
        {
            var personnel = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel];
            var action = _returnMessages[ReturnMessages.Action_Password];
            if (personnel == null)
                return Result.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            if (!_passwordHasher.VerifyItem(request.CurrentPassword, personnel.PasswordHash, personnel.PasswordSalt))
                return Result.Failure(_returnMessages[ReturnMessages.Error_InvalidCurrentPassword], StatusCode.InvalidCredentials, HttpStatusCode.BadRequest);

            try
            {
                var (newHash, newsalt) = _passwordHasher.HashItem(request.NewPassword);
                personnel.PasswordHash = newHash;
                personnel.PasswordSalt = newsalt;
                await _repository.UpdateAsync(personnel);
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
