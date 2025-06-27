using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Commands
{
    public class CreateBrandCommand : IRequest<Result<int>>
    {
        public string BrandName { get; set; }
    }
}
