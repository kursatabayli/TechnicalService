using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class SearchServiceRecordsQuery : IRequest<Result<List<ServiceRecordListResult>>>
    {
        public string SearchTerm { get; set; }

        public SearchServiceRecordsQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
