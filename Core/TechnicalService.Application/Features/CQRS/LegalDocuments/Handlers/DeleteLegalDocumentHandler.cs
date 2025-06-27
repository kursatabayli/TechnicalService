using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Handlers
{
    public class DeleteLegalDocumentHandler : IRequestHandler<DeleteLegalDocumentCommand, Result<int>>
    {
        private readonly IRepository<LegalDocument, int> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly ILogger<DeleteLegalDocumentCommand> _logger;

        public DeleteLegalDocumentHandler(IRepository<LegalDocument, int> repository, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages, ILogger<DeleteLegalDocumentCommand> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(DeleteLegalDocumentCommand request, CancellationToken ct)
        {
            var legalDocument = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_LegalDocument];
            if (legalDocument == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);
            try
            {
                await _repository.DeleteAsync(legalDocument);
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
