using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands
{
    public class UpdateServiceRecordStepCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string StepTitle { get; set; }
        public string StepDescription { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
