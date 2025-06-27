using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.DTOs.ServiceRecordDTOs
{
    public class UpdateServiceRecordDto
    {
        public Guid Id { get; set; }
        public ServiceStatusDto? Status { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
