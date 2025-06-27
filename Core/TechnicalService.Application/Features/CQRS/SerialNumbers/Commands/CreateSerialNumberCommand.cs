using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Commands
{
    public class CreateSerialNumberCommand : IRequest<Result<int>>
    {
        public string Serial_Number { get; set; }
        public int ProductId { get; set; }
        public DateOnly RegisterDate { get; set; }

    }
}
