using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.ServiceRecordDTOs
{
    public class ServiceRecordDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public Guid UserId { get; set; }
        public UserProductDto UserProduct { get; set; }
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ServiceStatusDto Status { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? PersonnelName { get; set; }

        public Guid? PersonnelId { get; set; }
    }
}
