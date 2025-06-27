using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Queries;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Handlers
{
    public class GetLegalDocumentByDocumentTypeQueryHandler : IRequestHandler<GetLegalDocumentByDocumentTypeQuery, Result<LegalDocumentResult>>
    {
        private readonly IRepository<LegalDocument, int> _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly ILogger<GetLegalDocumentByDocumentTypeQueryHandler> _logger;

        public GetLegalDocumentByDocumentTypeQueryHandler(IRepository<LegalDocument, int> repository, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages, ILogger<GetLegalDocumentByDocumentTypeQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _returnMessages = returnMessages;
            _logger = logger;
        }

        public async Task<Result<LegalDocumentResult>> Handle(GetLegalDocumentByDocumentTypeQuery request, CancellationToken ct)
        {
            var legalDocument = await _repository.GetFirstOrDefaultAsync(x => ((int)x.DocumentType) == request.DocumentType);
            var entityName = _returnMessages[ReturnMessages.EntityType_LegalDocument];

            if (legalDocument == null)
                return Result<LegalDocumentResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedLegalDocument = _mapper.Map<LegalDocumentResult>(legalDocument);
                return Result<LegalDocumentResult>.Success(mappedLegalDocument, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.DocumentType);
                return Result<LegalDocumentResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
