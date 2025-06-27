using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Handlers
{
    public class AddServiceRecordStepHandler : IRequestHandler<AddServiceRecordStepCommand, Result<int>>
    {
        private readonly IRepository<ServiceRecordStep, int> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddServiceRecordStepHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public AddServiceRecordStepHandler(IRepository<ServiceRecordStep, int> repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddServiceRecordStepHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(AddServiceRecordStepCommand request, CancellationToken cancellationToken)
        {
            var serviceRecordStep = _mapper.Map<ServiceRecordStep>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecordStep];
            var existingIncompleteServiceRecordStep = await _repository.GetFirstOrDefaultAsync(srs => srs.ServiceRecordId == serviceRecordStep.ServiceRecordId && srs.IsCompleted == false);


            try
            {
                if (existingIncompleteServiceRecordStep != null)
                {
                    existingIncompleteServiceRecordStep.IsCompleted = true;
                    existingIncompleteServiceRecordStep.CompletedDate = DateTime.UtcNow;
                    await _repository.UpdateAsync(existingIncompleteServiceRecordStep);
                }

                if (serviceRecordStep.IsCompleted)
                    serviceRecordStep.CompletedDate = DateTime.UtcNow;

                await _repository.CreateAsync(serviceRecordStep);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return Result<int>.Success(serviceRecordStep.Id, _returnMessages[ReturnMessages.Message_Success_Created, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName]);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
