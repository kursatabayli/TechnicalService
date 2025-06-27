using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Commands
{
    public class CreateServiceRecordCommand : IRequest<Result<Guid>>
    {
        public Guid UserId { get; set; }
        public int UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
