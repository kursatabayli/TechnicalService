using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Commands
{
    public class UpdateSerialNumberCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public int ProductId { get; set; }
        public DateOnly RegisterDate { get; set; }
    }
}
