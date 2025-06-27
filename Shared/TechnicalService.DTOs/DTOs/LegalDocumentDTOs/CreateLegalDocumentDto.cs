using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.LegalDocumentDTOs
{
    public class CreateLegalDocumentDto
    {
        public DocumentTypeDto? DocumentType { get; set; }
        public string Content { get; set; }
    }
}
