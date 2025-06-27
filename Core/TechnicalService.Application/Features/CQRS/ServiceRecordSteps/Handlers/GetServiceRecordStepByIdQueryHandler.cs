using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Queries;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Handlers
{
    public class GetServiceRecordStepByIdQueryHandler : IRequestHandler<GetServiceRecordStepByIdQuery, Result<ServiceRecordStepResult>>
    {
        private readonly IRepository<ServiceRecordStep, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServiceRecordStepsByServiceRecordIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetServiceRecordStepByIdQueryHandler(IRepository<ServiceRecordStep, int> repository, IMapper mapper, ILogger<GetServiceRecordStepsByServiceRecordIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<ServiceRecordStepResult>> Handle(GetServiceRecordStepByIdQuery request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecordStep_Singular];
            try
            {
                var serviceRecordStep = await _repository.GetByIdAsync(request.Id);

                var mappedStep = _mapper.Map<ServiceRecordStepResult>(serviceRecordStep);
                return Result<ServiceRecordStepResult>.Success(mappedStep, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<ServiceRecordStepResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
