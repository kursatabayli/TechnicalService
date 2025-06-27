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
    internal class GetAllLegalDocumentsQueryHandler : IRequestHandler<GetAllLegalDocumentsQuery, Result<List<LegalDocumentResult>>>
    {
        private readonly IRepository<LegalDocument, int> _repository;
        private readonly ILogger<GetAllLegalDocumentsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllLegalDocumentsQueryHandler(IRepository<LegalDocument, int> repository, ILogger<GetAllLegalDocumentsQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }
        public async Task<Result<List<LegalDocumentResult>>> Handle(GetAllLegalDocumentsQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_LegalDocument_Plural];
            try
            {
                var legalDocuments = await _repository.GetAllAsync();
                var mappedLegalDocuments = _mapper.Map<List<LegalDocumentResult>>(legalDocuments);
                return Result<List<LegalDocumentResult>>.Success(mappedLegalDocuments, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<LegalDocumentResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
