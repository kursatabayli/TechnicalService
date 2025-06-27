using TechnicalService.Domain.Enums;

namespace TechnicalService.Domain.Entities
{
    public class ServiceRecord
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public UserProduct UserProduct { get; set; }
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ServiceStatus Status { get; set; } = ServiceStatus.Pending;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedDate { get; set; }
        public Personnel? Personnel { get; set; }
        public Guid? PersonnelId { get; set; }

        public List<ServiceRecordStep> ServiceRecordSteps { get; set; }
    }
}
