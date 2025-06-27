using MediatR;
using TechnicalService.Application.Features.CQRS.Personnels.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Queries
{
    public class GetPersonnelByIdQuery : IRequest<Result<PersonnelResult>>
    {
        public Guid Id { get; set; }
        public GetPersonnelByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
