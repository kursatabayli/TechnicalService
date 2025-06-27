using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Handlers
{
    public class UpdateServiceRecordStepHandler : IRequestHandler<UpdateServiceRecordStepCommand, Result<int>>
    {
        private readonly IRepository<ServiceRecordStep, int> _repository;
        private readonly ILogger<UpdateServiceRecordStepHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public UpdateServiceRecordStepHandler(IRepository<ServiceRecordStep, int> repository, ILogger<UpdateServiceRecordStepHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateServiceRecordStepCommand request, CancellationToken cancellationToken)
        {
            var serviceRecordStep = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecordStep];

            if(serviceRecordStep == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                _mapper.Map(request, serviceRecordStep);
                if(serviceRecordStep.IsCompleted)
                    serviceRecordStep.CompletedDate = DateTime.UtcNow;
                else
                    serviceRecordStep.CompletedDate = null;
                await _repository.UpdateAsync(serviceRecordStep);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(serviceRecordStep.Id, _returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
