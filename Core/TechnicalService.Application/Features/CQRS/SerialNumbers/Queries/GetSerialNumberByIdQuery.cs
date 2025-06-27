using TechnicalService.Application.Features.CQRS.SerialNumbers.Results;
using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Queries
{
    public class GetSerialNumberByIdQuery : IRequest<Result<SerialNumberResult>>
    {
        public int Id { get; set; }
        public GetSerialNumberByIdQuery(int id)
        {
            Id = id;
        }
    }
}
