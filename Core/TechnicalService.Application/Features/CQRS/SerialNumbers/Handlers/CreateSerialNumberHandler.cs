using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Handlers
{
    public class CreateSerialNumberHandler : IRequestHandler<CreateSerialNumberCommand, Result<int>>
    {
        private readonly IRepository<SerialNumber, int> _repository;
        private readonly ILogger<CreateSerialNumberHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public CreateSerialNumberHandler(IRepository<SerialNumber, int> repository, ILogger<CreateSerialNumberHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(CreateSerialNumberCommand request, CancellationToken ct)
        {
            var serialNumber = _mapper.Map<SerialNumber>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_SerialNumber];

            var existingSerialNumber = await _repository.GetFirstOrDefaultAsync(x => x.Serial_Number == serialNumber.Serial_Number);

            if (existingSerialNumber != null)
                return Result<int>.Failure(existingSerialNumber.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, existingSerialNumber.Serial_Number, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                await _repository.CreateAsync(serialNumber);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(serialNumber.Id, _returnMessages[ReturnMessages.Message_Success_Created_WithName, serialNumber.Serial_Number, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName], request.Serial_Number);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
