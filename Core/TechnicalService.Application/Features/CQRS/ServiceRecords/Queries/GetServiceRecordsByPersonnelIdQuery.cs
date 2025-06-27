using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class GetServiceRecordsByPersonnelIdQuery : IRequest<Result<List<ServiceRecordListResult>>>
    {
        public Guid PersonnelId { get; set; }

        public GetServiceRecordsByPersonnelIdQuery(Guid personnelId)
        {
            PersonnelId = personnelId;
        }
    }
}
