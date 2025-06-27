using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.ServiceRecordDTOs
{
    public class UserServiceRecordsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Serial_Number { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public DateOnly WarrantyDate { get; set; } // in months
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ServiceStatusDto Status { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
