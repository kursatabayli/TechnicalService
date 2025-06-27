using MediatR;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Results;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Queries
{
    public class GetLegalDocumentByDocumentTypeQuery : IRequest<Result<LegalDocumentResult>>
    {
        public int DocumentType { get; set; }

        public GetLegalDocumentByDocumentTypeQuery(int documentType)
        {
            DocumentType = documentType;
        }
    }
}
