using MediatR;
using TechnicalService.DTOs.Response;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Commands
{
    public class AddUserProductCommand : IRequest<Result<int>>
    {
        public int SerialNumberId { get; set; }
        public string SerialNumber { get; set; }
        public Guid UserId { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly WarrantyDate { get; set; }
    }
}
