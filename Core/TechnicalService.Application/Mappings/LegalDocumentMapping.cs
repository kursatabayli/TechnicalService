using AutoMapper;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Commands;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;

namespace TechnicalService.Application.Mappings
{
    internal class LegalDocumentMapping : Profile
    {
        public LegalDocumentMapping() 
        {
            CreateMap<LegalDocument, LegalDocumentResult>();
            CreateMap<LegalDocumentResult, LegalDocumentDto>();

            CreateMap<CreateLegalDocumentCommand, LegalDocument>();
            CreateMap<CreateLegalDocumentDto, CreateLegalDocumentCommand>();

            CreateMap<UpdateLegalDocumentCommand, LegalDocument>();
            CreateMap<UpdateLegalDocumentDto, UpdateLegalDocumentCommand>();

        }
    }
}
