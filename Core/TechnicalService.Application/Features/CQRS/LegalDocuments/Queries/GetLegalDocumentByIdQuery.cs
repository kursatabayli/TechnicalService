using MediatR;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Queries
{
    public class GetLegalDocumentByIdQuery : IRequest<Result<LegalDocumentResult>>
    {
        public int Id { get; set; }
        public GetLegalDocumentByIdQuery(int id)
        {
            Id = id;
        }
    }
}
