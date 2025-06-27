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
    public class UpdateSerialNumberHandler : IRequestHandler<UpdateSerialNumberCommand, Result<int>>
    {
        private readonly IRepository<SerialNumber, int> _repository;
        private readonly ILogger<UpdateSerialNumberHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public UpdateSerialNumberHandler(IRepository<SerialNumber, int> repository, ILogger<UpdateSerialNumberHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateSerialNumberCommand request, CancellationToken ct)
        {
            var serialNumber = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_SerialNumber];

            if (serialNumber == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingSerialNumberWithSameName = await _repository.GetFirstOrDefaultAsync(x => x.Serial_Number == request.Serial_Number && x.Id != request.Id);

            if (existingSerialNumberWithSameName != null)
                return Result<int>.Failure(existingSerialNumberWithSameName.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.Serial_Number, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, serialNumber);
                await _repository.UpdateAsync(serialNumber);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(serialNumber.Id, _returnMessages[ReturnMessages.Message_Success_Updated_WithName, serialNumber.Serial_Number, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
