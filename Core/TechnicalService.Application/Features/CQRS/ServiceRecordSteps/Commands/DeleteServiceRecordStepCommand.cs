using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands
{
    public class DeleteServiceRecordStepCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public DeleteServiceRecordStepCommand(int id)
        {
            Id = id;
        }
    }
}
