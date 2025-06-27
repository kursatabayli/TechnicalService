using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Queries;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Handlers
{
    public class SearchServiceRecordsQueryHandler : IRequestHandler<SearchServiceRecordsQuery, Result<List<ServiceRecordListResult>>>
    {
        private readonly IServiceRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchServiceRecordsQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public SearchServiceRecordsQueryHandler(IServiceRecordRepository repository, IMapper mapper, ILogger<SearchServiceRecordsQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }
        public async Task<Result<List<ServiceRecordListResult>>> Handle(SearchServiceRecordsQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord_Plural];

            try
            {
                var serviceRecords = await _repository.SearchServiceRecordQuery(request.SearchTerm);
                var mappedServiceRecords = _mapper.Map<List<ServiceRecordListResult>>(serviceRecords);
                return Result<List<ServiceRecordListResult>>.Success(mappedServiceRecords, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<ServiceRecordListResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
