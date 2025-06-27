using MediatR;
using TechnicalService.Application.Features.CQRS.Personnels.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Queries
{
    public class GetPersonnelsByServiceQuery : IRequest<Result<List<PersonnelMinimalResult>>>
    {
        public Guid PersonnelId { get; set; }

        public GetPersonnelsByServiceQuery(Guid personnelId)
        {
            PersonnelId = personnelId;
        }
    }
}
