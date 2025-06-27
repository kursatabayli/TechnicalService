using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands
{
    public class AddServiceRecordStepCommand : IRequest<Result<int>>
    {
        public Guid ServiceRecordId { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
