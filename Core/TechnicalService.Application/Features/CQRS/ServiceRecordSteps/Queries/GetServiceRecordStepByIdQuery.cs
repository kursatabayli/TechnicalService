using MediatR;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Queries
{
    public class GetServiceRecordStepByIdQuery : IRequest<Result<ServiceRecordStepResult>>
    {
        public int Id { get; set; }
        public GetServiceRecordStepByIdQuery(int id)
        {
            Id = id;
        }
    }
}
