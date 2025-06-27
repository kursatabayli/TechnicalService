using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.ServiceRecordDTOs
{
    public class ServiceRecordListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserFullName { get; set; }
        public string SerialNumber { get; set; }
        public string ProductDetail { get; set; }
        public ServiceStatusDto Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
