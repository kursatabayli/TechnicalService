using MediatR;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Queries
{
    public class GetTechnicalServiceByIdQuery : IRequest<Result<TechnicalServiceResult>>
    {
        public int Id { get; set; }

        public GetTechnicalServiceByIdQuery(int id)
        {
            Id = id;
        }
    }
}
