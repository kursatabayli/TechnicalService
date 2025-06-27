using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Results
{
    public class LegalDocumentResult
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
