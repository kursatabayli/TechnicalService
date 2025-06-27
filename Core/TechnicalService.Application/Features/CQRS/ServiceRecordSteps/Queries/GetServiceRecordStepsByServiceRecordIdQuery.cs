using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Queries
{
    public class GetServiceRecordStepsByServiceRecordIdQuery : IRequest<Result<List<ServiceRecordStepResult>>>
    {
        public Guid ServiceRecordId { get; set; }

        public GetServiceRecordStepsByServiceRecordIdQuery(Guid serviceRecordId)
        {
            ServiceRecordId = serviceRecordId;
        }
    }
}
