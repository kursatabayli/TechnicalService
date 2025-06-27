using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Commands;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Handlers
{
    public class DeleteTechnicalServiceHandler : IRequestHandler<DeleteTechnicalServiceCommand, Result<int>>
    {
        private readonly IRepository<Domain.Entities.TechnicalService, int> _repository;
        private readonly ILogger<DeleteTechnicalServiceHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public DeleteTechnicalServiceHandler(IRepository<Domain.Entities.TechnicalService, int> repository, ILogger<DeleteTechnicalServiceHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(DeleteTechnicalServiceCommand request, CancellationToken ct)
        {
            var technicalService = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_TechnicalService];

            if (technicalService == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                await _repository.DeleteAsync(technicalService);
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
