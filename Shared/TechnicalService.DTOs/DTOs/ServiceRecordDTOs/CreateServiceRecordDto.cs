using TechnicalService.DTOs.DTOs.UserProductDTOs;

namespace TechnicalService.DTOs.DTOs.ServiceRecordDTOs
{
    public class CreateServiceRecordDto
    {
        public Guid UserId { get; set; }
        public UserProductDto UserProduct { get; set; }
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
