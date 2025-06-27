using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Queries;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Handlers
{
    public class GetServiceRecordStepsByServiceRecordIdQueryHandler : IRequestHandler<GetServiceRecordStepsByServiceRecordIdQuery, Result<List<ServiceRecordStepResult>>>
    {
        private readonly IServiceRecordStepsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServiceRecordStepsByServiceRecordIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetServiceRecordStepsByServiceRecordIdQueryHandler(IServiceRecordStepsRepository repository, IMapper mapper, ILogger<GetServiceRecordStepsByServiceRecordIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<ServiceRecordStepResult>>> Handle(GetServiceRecordStepsByServiceRecordIdQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecordStep_Plural];

            try
            {
                var serviceRecordSteps = await _repository.GetAllServiceRecordStepsWithPersonnelByServiceRecordId(request.ServiceRecordId);

                var mappedSteps = _mapper.Map<List<ServiceRecordStepResult>>(serviceRecordSteps);

                return Result<List<ServiceRecordStepResult>>.Success(mappedSteps, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName], request.ServiceRecordId);

                return Result<List<ServiceRecordStepResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
