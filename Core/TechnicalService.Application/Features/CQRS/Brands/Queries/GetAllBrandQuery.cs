using TechnicalService.Application.Features.CQRS.Brands.Results;
using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Queries
{
    public class GetAllBrandQuery : IRequest<Result<List<BrandResult>>>
    {
    }
}
