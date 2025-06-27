using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class GetAllServiceRecordsQuery : IRequest<Result<List<ServiceRecordListResult>>>
    {
        public GetAllServiceRecordsQuery()
        {
        }
    }
}
