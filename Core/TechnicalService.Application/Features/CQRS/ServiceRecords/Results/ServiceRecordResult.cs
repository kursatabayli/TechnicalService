using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Results
{
    public class ServiceRecordResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ServiceStatus Status { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? PersonnelName { get; set; }

        public Guid? PersonnelId { get; set; }

    }
}
