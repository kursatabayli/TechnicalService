using TechnicalService.Application.Features.CQRS.SerialNumbers.Results;
using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Queries
{
    public class GetAllSerialNumbersQuery : IRequest<Result<List<SerialNumberResult>>>
    {
    }
}
