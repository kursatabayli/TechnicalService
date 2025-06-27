using MediatR;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Queries
{
    public class GetAllTechnicalServicesQuery : IRequest<Result<List<TechnicalServiceResult>>>
    {
    }
}
