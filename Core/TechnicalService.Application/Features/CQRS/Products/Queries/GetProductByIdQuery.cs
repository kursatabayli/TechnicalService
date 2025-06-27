using TechnicalService.Application.Features.CQRS.Products.Results;
using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Result<ProductResult>>
    {
        public int Id { get; set; }
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
