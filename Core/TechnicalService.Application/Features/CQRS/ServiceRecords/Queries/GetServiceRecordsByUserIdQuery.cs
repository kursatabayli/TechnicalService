using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class GetServiceRecordsByUserIdQuery : IRequest<Result<List<UserServiceRecordResult>>>
    {
        public Guid UserId { get; set; }

        public GetServiceRecordsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
