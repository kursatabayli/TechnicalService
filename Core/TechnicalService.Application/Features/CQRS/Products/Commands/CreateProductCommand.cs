using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Products.Commands
{
    public class CreateProductCommand : IRequest<Result<int>>
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
    }
}
