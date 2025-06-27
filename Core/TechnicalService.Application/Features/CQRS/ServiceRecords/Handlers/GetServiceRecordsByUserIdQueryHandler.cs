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
    public class GetServiceRecordsByUserIdQueryHandler : IRequestHandler<GetServiceRecordsByUserIdQuery, Result<List<UserServiceRecordResult>>>
    {
        private readonly IServiceRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServiceRecordsByUserIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetServiceRecordsByUserIdQueryHandler(IServiceRecordRepository repository, IMapper mapper, ILogger<GetServiceRecordsByUserIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<UserServiceRecordResult>>> Handle(GetServiceRecordsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord_Plural];

            try
            {
                var serviceRecords = await _repository.GetAllServiceRecordsByUserIdAsync(request.UserId);

                var mappedUserRecords = _mapper.Map<List<UserServiceRecordResult>>(serviceRecords);
                return Result<List<UserServiceRecordResult>>.Success(mappedUserRecords, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<UserServiceRecordResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
