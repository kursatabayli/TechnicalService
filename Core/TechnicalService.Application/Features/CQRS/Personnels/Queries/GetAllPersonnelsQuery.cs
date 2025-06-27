using MediatR;
using TechnicalService.Application.Features.CQRS.Personnels.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Queries
{
    public class GetAllPersonnelsQuery : IRequest<Result<List<PersonnelResult>>>
    {
        public GetAllPersonnelsQuery()
        {
        }
    }
}
