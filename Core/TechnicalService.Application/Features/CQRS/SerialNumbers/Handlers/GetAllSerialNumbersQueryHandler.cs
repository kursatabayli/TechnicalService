using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Queries;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Handlers
{
    public class GetAllSerialNumbersQueryHandler : IRequestHandler<GetAllSerialNumbersQuery, Result<List<SerialNumberResult>>>
    {
        private readonly ISerialNumberRepository _repository;
        private readonly ILogger<GetAllSerialNumbersQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllSerialNumbersQueryHandler(ISerialNumberRepository repository, ILogger<GetAllSerialNumbersQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<SerialNumberResult>>> Handle(GetAllSerialNumbersQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_SerialNumber_Plural];

            try
            {
                var serialNumbers = await _repository.GetAllSerialNumbersAsync();
                var mappedSerialNumbers = _mapper.Map<List<SerialNumberResult>>(serialNumbers);
                return Result<List<SerialNumberResult>>.Success(mappedSerialNumbers, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<SerialNumberResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
