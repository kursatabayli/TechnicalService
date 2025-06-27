using MediatR;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Commands
{
    public class UpdateServiceRecordCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public ServiceStatus? Status { get; set; }
        public Guid? PersonnelId { get; set; }
    }
}
