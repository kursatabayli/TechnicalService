namespace TechnicalService.Domain.Entities
{
    public class ServiceRecordStep
    {
        public int Id { get; set; }
        public ServiceRecord ServiceRecord { get; set; }
        public Guid ServiceRecordId { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }
        public Personnel? Personnel { get; set; }
        public Guid? PersonnelId { get; set; }

    }
}
