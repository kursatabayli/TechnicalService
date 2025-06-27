namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results
{
    public class ServiceRecordStepResult
    {
        public int Id { get; set; }
        public Guid ServiceRecordId { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? PersonnelFullName { get; set; }
    }
}
