using TechnicalService.Application.Features.CQRS.Products.Results;
using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Products.Queries
{
    public class GetAllProductQuery : IRequest<Result<List<ProductResult>>>
    {
    }
}
