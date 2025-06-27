using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.LegalDocumentDTOs
{
    public class UpdateLegalDocumentDto
    {
        public int Id { get; set; }
        public DocumentTypeDto? DocumentType { get; set; }
        public string Content { get; set; }
    }
}
