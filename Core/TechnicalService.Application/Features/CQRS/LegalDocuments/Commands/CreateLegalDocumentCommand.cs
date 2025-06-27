using MediatR;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Commands
{
    public class CreateLegalDocumentCommand : IRequest<Result<int>>
    {
        public DocumentType DocumentType { get; set; }
        public string Content { get; set; }
    }
}
