using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class GetServiceRecordByIdQuery : IRequest<Result<ServiceRecordResult>>
    {
        public Guid Id { get; set; }
        public GetServiceRecordByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
