using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Handlers
{
    public class CreateServiceRecordHandler : IRequestHandler<CreateServiceRecordCommand, Result<Guid>>
    {
        private readonly IRepository<ServiceRecord, Guid> _repository;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateServiceRecordHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly IEmailService _emailService;
        public CreateServiceRecordHandler(IRepository<ServiceRecord, Guid> repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateServiceRecordHandler> logger, IStringLocalizer<ReturnMessages> returnMessages, IEmailService emailService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
            _emailService = emailService;
        }

        public async Task<Result<Guid>> Handle(CreateServiceRecordCommand request, CancellationToken cancellationToken)
        {
            var serviceRecord = _mapper.Map<ServiceRecord>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord];

            var existingIncompleteServiceRecord = await _repository.GetFirstOrDefaultAsync(sr => sr.UserProductId == serviceRecord.UserProductId && sr.IsCompleted == false);
            var existingUser = await _userRepository.GetByIdAsync(request.UserId);
            if (existingIncompleteServiceRecord != null)
                return Result<Guid>.Failure(existingIncompleteServiceRecord.Id, _returnMessages[ReturnMessages.Error_ServiceRecord_IncompleteExists_ForProduct], StatusCode.Conflict, HttpStatusCode.Conflict);
            try
            {
                await _repository.CreateAsync(serviceRecord);
                await _unitOfWork.SaveChangesAsync();
                await _emailService.SendServiceRecordCreateNotifyEmailAsync(existingUser.Email, serviceRecord.Id.ToString()[..8].ToUpper(), serviceRecord.Id.ToString());
                return Result<Guid>.Success(serviceRecord.Id, _returnMessages[ReturnMessages.Message_Success_Created, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName]);
                return Result<Guid>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
