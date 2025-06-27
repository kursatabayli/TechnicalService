using MediatR;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.LegalDocuments.Commands
{
    public class DeleteLegalDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteLegalDocumentCommand(int id)
        {
            Id = id;
        }
    }
}
