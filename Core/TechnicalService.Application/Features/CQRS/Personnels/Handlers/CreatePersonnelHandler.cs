using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Personnels.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Handlers
{
    public class CreatePersonnelHandler : IRequestHandler<CreatePersonnelCommand, Result<Guid>>
    {
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly ILogger<CreatePersonnelHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public CreatePersonnelHandler(IRepository<Personnel, Guid> repository, ILogger<CreatePersonnelHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IHashService passwordHasher, IEmailService emailService, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _returnMessages = returnMessages;
        }

        public async Task<Result<Guid>> Handle(CreatePersonnelCommand request, CancellationToken cancellationToken)
        {
            if (request.Role == Role.User)
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Personnel_Role_Conflict], StatusCode.NotAcceptable, HttpStatusCode.NotAcceptable);

            var personnel = _mapper.Map<Personnel>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel];

            var validationResult = await ValidatePersonnelData(request);

            if (!string.IsNullOrEmpty(validationResult.Item1) && validationResult.Item2 != Guid.Empty)
                return Result<Guid>.Failure(validationResult.Item2, validationResult.Item1, StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                var createdPassword = _passwordHasher.GeneratePassword();
                var (hash, salt) = _passwordHasher.HashItem(createdPassword);
                personnel.PasswordHash = hash;
                personnel.PasswordSalt = salt;
                personnel.Id = Guid.NewGuid();

                await _repository.CreateAsync(personnel);
                await _unitOfWork.SaveChangesAsync();

                await _emailService.SendPersonnelRegistrationEmailAsync(request.Email, request.InternalEmail, createdPassword);
                return Result<Guid>.Success(personnel.Id, _returnMessages[ReturnMessages.Personnel_Created_Success_And_EmailSent]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName]);
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<(string, Guid)> ValidatePersonnelData(CreatePersonnelCommand request)
        {
            var existingPersonnel = await _repository.GetFirstOrDefaultAsync(x => x.IdentityNumber == request.IdentityNumber || x.Email == request.Email || x.InternalEmail == request.InternalEmail);

            if (existingPersonnel != null)
                switch (true)
                {
                    case var _ when existingPersonnel.IdentityNumber == request.IdentityNumber:
                        return (_returnMessages[ReturnMessages.Personnel_Identity_Conflict], existingPersonnel.Id);
                    case var _ when existingPersonnel.InternalEmail == request.InternalEmail:
                        return (_returnMessages[ReturnMessages.Personnel_InternalEmail_Conflict], existingPersonnel.Id);
                    //case var _ when existingPersonnel.Email == request.Email:
                    //    return (_returnMessages[ReturnMessages.Personnel_Email_Conflict], existingPersonnel.Id);
                }
            return (string.Empty, Guid.Empty);
        }
    }
}
