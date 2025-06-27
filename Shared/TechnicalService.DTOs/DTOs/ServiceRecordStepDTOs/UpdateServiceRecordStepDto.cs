namespace TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs
{
    public class UpdateServiceRecordStepDto
    {
        public int Id { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
