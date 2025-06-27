using MediatR;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Commands
{
    public class DeleteServiceRecordCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public DeleteServiceRecordCommand(Guid id)
        {
            Id = id;
        }
    }
}
