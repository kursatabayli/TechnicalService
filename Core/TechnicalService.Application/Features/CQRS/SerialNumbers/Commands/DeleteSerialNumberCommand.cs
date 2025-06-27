using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Commands
{
    public class DeleteSerialNumberCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public DeleteSerialNumberCommand(int id)
        {
            Id = id;
        }
    }
}
