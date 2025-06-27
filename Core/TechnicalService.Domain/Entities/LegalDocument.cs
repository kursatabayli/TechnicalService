using TechnicalService.Domain.Enums;

namespace TechnicalService.Domain.Entities
{
    public class LegalDocument
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
