using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Results
{
    public class ServiceRecordListResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserFullName { get; set; }
        public string SerialNumber { get; set; }
        public string ProductDetail { get; set; }
        public ServiceStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
