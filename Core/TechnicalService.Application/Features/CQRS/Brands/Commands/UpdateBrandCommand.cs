using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Commands
{
    public class UpdateBrandCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
    }
}
