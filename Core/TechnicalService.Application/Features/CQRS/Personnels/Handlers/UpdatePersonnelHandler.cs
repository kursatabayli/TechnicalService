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
    public class UpdatePersonnelHandler : IRequestHandler<UpdatePersonnelCommand, Result<Guid>>
    {
        private readonly IRepository<Personnel, Guid> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly ILogger<UpdatePersonnelHandler> _logger;
        public UpdatePersonnelHandler(IRepository<Personnel, Guid> repository, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages, ILogger<UpdatePersonnelHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(UpdatePersonnelCommand request, CancellationToken cancellationToken)
        {
            if (request.Role == Role.User)
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Personnel_Role_Conflict], StatusCode.NotAcceptable, HttpStatusCode.NotAcceptable);

            var personnel = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel];
            var validationResult = await ValidatePersonnelData(request);

            if (!string.IsNullOrEmpty(validationResult.Item1) && validationResult.Item2 != Guid.Empty)
                return Result<Guid>.Failure(validationResult.Item2, validationResult.Item1, StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, personnel);
                await _repository.UpdateAsync(personnel);
                await _unitOfWork.SaveChangesAsync();
                return Result<Guid>.Success(personnel.Id, _returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<(string, Guid)> ValidatePersonnelData(UpdatePersonnelCommand request)
        {
            var existingPersonnel = await _repository.GetFirstOrDefaultAsync(x => x.Id != request.Id && (x.IdentityNumber == request.IdentityNumber || x.Email == request.Email || x.InternalEmail == request.InternalEmail));

            if (existingPersonnel != null)
            {
                switch (true)
                {
                    case var _ when existingPersonnel.IdentityNumber == request.IdentityNumber:
                        return (_returnMessages[ReturnMessages.Personnel_Identity_Conflict], existingPersonnel.Id);
                    case var _ when existingPersonnel.InternalEmail == request.InternalEmail:
                        return (_returnMessages[ReturnMessages.Personnel_InternalEmail_Conflict], existingPersonnel.Id);
                    case var _ when existingPersonnel.Email == request.Email:
                        return (_returnMessages[ReturnMessages.Personnel_Email_Conflict], existingPersonnel.Id);
                }
            }

            return (string.Empty, Guid.Empty);
        }
    }
}
