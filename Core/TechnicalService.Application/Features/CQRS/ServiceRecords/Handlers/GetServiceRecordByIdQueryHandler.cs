using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Queries;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Handlers
{
    public class GetServiceRecordByIdQueryHandler : IRequestHandler<GetServiceRecordByIdQuery, Result<ServiceRecordResult>>
    {
        private readonly IServiceRecordRepository _repository;
        private readonly ILogger<GetServiceRecordByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetServiceRecordByIdQueryHandler(IServiceRecordRepository repository, ILogger<GetServiceRecordByIdQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<ServiceRecordResult>> Handle(GetServiceRecordByIdQuery request, CancellationToken ct)
        {
            var serviceRecord = await _repository.GetServiceRecordByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord];

            if (serviceRecord == null)
                return Result<ServiceRecordResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedServiceRecord = _mapper.Map<ServiceRecordResult>(serviceRecord);
                return Result<ServiceRecordResult>.Success(mappedServiceRecord, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<ServiceRecordResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
