using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Commands
{
    public class DeleteTechnicalServiceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public DeleteTechnicalServiceCommand(int id)
        {
            Id = id;
        }
    }
}
