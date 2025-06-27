using AutoMapper;
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
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<Guid>>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IRepository<EmailVerificationToken, Guid> _tokenRepository;
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public UpdateUserHandler(IRepository<User, Guid> repository, ILogger<UpdateUserHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IRepository<EmailVerificationToken, Guid> tokenRepository, IAuthService authService, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenRepository = tokenRepository;
            _authService = authService;
            _returnMessages = returnMessages;
        }

        public async Task<Result<Guid>> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_User];

            if (user == null)
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingUser = await _repository.GetFirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower() && x.Id != request.Id);

            if (existingUser != null && existingUser.Id != request.Id)
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_User_EmailAlreadyExists], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {

                if (user.Email != request.Email)
                {
                    user.IsEmailConfirmed = false;
                    var verificationToken = _authService.GenerateEmailVerificationToken(user.Email);

                    var writeTokenOnDb = new EmailVerificationToken
                    {
                        Token = verificationToken,
                        UserId = user.Id,
                        Expiry = DateTime.UtcNow.AddHours(1),
                    };
                    await _tokenRepository.CreateAsync(writeTokenOnDb);
                }

                _mapper.Map(request, user);
                await _repository.UpdateAsync(user);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return Result<Guid>.Success(user.Id, _returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
