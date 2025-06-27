using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Commands
{
    public class DeleteBrandCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public DeleteBrandCommand(int id)
        {
            Id = id;
        }
    }
}
