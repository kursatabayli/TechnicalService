using AutoMapper;
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
    public class UpdateLegalDocumentHandler : IRequestHandler<UpdateLegalDocumentCommand, Result<int>>
    {
        private readonly IRepository<LegalDocument, int> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly ILogger<UpdateLegalDocumentHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateLegalDocumentHandler(IRepository<LegalDocument, int> repository, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages, ILogger<UpdateLegalDocumentHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateLegalDocumentCommand request, CancellationToken ct)
        {
            var legalDocument = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_LegalDocument];
            if (legalDocument == null)
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);
            var existingLegalDocument = await _repository.GetFirstOrDefaultAsync(ld => ld.DocumentType == request.DocumentType && ld.Id != request.Id);

            if (existingLegalDocument != null)
                return Result<int>.Failure(existingLegalDocument.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.DocumentType.GetDescription(), entityName], StatusCode.Conflict, HttpStatusCode.Conflict);
            try
            {
                _mapper.Map(request, legalDocument);
                await _repository.UpdateAsync(legalDocument);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(legalDocument.Id, _returnMessages[ReturnMessages.Message_Success_Updated_WithName, legalDocument.DocumentType.GetDescription(), entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
}
}
