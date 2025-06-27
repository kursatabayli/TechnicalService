using MediatR;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Commands
{
    public class UpdateLegalDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Content { get; set; }
    }
}
