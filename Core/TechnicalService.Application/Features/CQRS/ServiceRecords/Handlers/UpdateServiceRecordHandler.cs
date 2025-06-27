using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Handlers
{
    public class UpdateServiceRecordHandler : IRequestHandler<UpdateServiceRecordCommand, Result>
    {
        private readonly IRepository<ServiceRecord, Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateServiceRecordHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public UpdateServiceRecordHandler(IRepository<ServiceRecord, Guid> repository, IMapper mapper, ILogger<UpdateServiceRecordHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result> Handle(UpdateServiceRecordCommand request, CancellationToken ct)
        {
            var serviceRecord = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord];

            if (serviceRecord == null)
                return Result.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                if (request.Status == ServiceStatus.Completed && !serviceRecord.IsCompleted)
                {
                    serviceRecord.CompletedDate = DateTime.UtcNow;
                    serviceRecord.IsCompleted = true;
                }

                _mapper.Map(request, serviceRecord);
                await _repository.UpdateAsync(serviceRecord);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success(_returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
