namespace TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs
{
    public class AddServiceRecordStepDto
    {
        public Guid ServiceRecordId { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
