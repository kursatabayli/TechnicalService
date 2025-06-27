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
    public class GetSerialNumberByIdQueryHandler : IRequestHandler<GetSerialNumberByIdQuery, Result<SerialNumberResult>>
    {
        private readonly ISerialNumberRepository _repository;
        private readonly ILogger<GetSerialNumberByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetSerialNumberByIdQueryHandler(ISerialNumberRepository repository, ILogger<GetSerialNumberByIdQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<SerialNumberResult>> Handle(GetSerialNumberByIdQuery request, CancellationToken ct)
        {
            var serialNumber = await _repository.GetSerialNumberByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_SerialNumber];

            if (serialNumber == null)
                return Result<SerialNumberResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedSerialNumber = _mapper.Map<SerialNumberResult>(serialNumber);
                return Result<SerialNumberResult>.Success(mappedSerialNumber, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<SerialNumberResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
