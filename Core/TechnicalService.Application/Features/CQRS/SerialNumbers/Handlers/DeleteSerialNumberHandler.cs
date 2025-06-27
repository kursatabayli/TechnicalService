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
    public class DeleteSerialNumberHandler : IRequestHandler<DeleteSerialNumberCommand, Result<int>>
    {
        private readonly IRepository<SerialNumber, int> _repository;
        private readonly ILogger<DeleteSerialNumberHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public DeleteSerialNumberHandler(IRepository<SerialNumber, int> repository, ILogger<DeleteSerialNumberHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(DeleteSerialNumberCommand request, CancellationToken ct)
        {
            var serialNumber = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_SerialNumber];

            if (serialNumber == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                await _repository.DeleteAsync(serialNumber);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(request.Id, _returnMessages[ReturnMessages.Message_Success_Deleted, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Delete, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Delete, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
