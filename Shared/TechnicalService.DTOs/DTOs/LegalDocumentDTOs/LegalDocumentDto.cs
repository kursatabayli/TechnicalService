using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.LegalDocumentDTOs
{
    public class LegalDocumentDto
    {
        public int Id { get; set; }
        public DocumentTypeDto DocumentType { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
