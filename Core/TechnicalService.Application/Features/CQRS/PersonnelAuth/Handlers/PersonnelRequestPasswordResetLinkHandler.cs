using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.PersonnelAuth.Handlers
{
    public class PersonnelRequestPasswordResetLinkHandler : IRequestHandler<PersonnelRequestPasswordResetLinkCommand, Result>
    {
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly IEmailService _emailService;
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonnelRequestPasswordResetLinkHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public PersonnelRequestPasswordResetLinkHandler(IRepository<Personnel, Guid> repository, IEmailService emailService, IHashService hashService, ILogger<PersonnelRequestPasswordResetLinkHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
            _hashService = hashService;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result> Handle(PersonnelRequestPasswordResetLinkCommand request, CancellationToken cancellationToken)
        {
            var personnel = await _repository.GetFirstOrDefaultAsync(x => x.IdentityNumber == request.IdentityNumber);
            if (personnel == null || !ValidatePersonnelInfo(request, personnel))
                return Result.Failure(_returnMessages[ReturnMessages.Password_Reset_InfoMismatch], StatusCode.NotFound, HttpStatusCode.NotFound);
            var action = _returnMessages[ReturnMessages.Action_PasswordReset];
            try
            {
                var newPassword = _hashService.GeneratePassword();
                var (passwordHash, passwordSalt) = _hashService.HashItem(newPassword);
                personnel.PasswordHash = passwordHash;
                personnel.PasswordSalt = passwordSalt;
                await _repository.UpdateAsync(personnel);
                await _unitOfWork.SaveChangesAsync();
                await _emailService.SendPersonnelNewPasswordEmailAsync(personnel.Email, personnel.InternalEmail, newPassword);
                return Result.Success(_returnMessages[ReturnMessages.Password_Reset_SuccessEmailSent, request.Email]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action]);
                return Result.Failure(_returnMessages[ReturnMessages.Error_Generic_SimpleOperationFailed, action, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }

        }

        private static bool ValidatePersonnelInfo(PersonnelRequestPasswordResetLinkCommand request, Personnel personnel)
        {
            if (!string.Equals(request.Name, personnel.Name, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(request.Surname, personnel.Surname, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(request.Email, personnel.Email, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(request.InternalEmail, personnel.InternalEmail, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(request.IdentityNumber, personnel.IdentityNumber, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!personnel.BirthDate.HasValue || request.BirthDate != personnel.BirthDate.Value)
                return false;

            return true;
        }

    }
}
