using MediatR;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Queries
{
    public class GetAllLegalDocumentsQuery : IRequest<Result<List<LegalDocumentResult>>>
    {
    }
}
