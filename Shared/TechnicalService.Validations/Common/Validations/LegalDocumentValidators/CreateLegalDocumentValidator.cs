using FluentValidation;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;

namespace TechnicalService.Validations.Common.Validations.LegalDocumentValidators
{
    public class CreateLegalDocumentValidator : AbstractValidatorBase<CreateLegalDocumentDto>
    {
        public CreateLegalDocumentValidator()
        {
            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("Belge türü zorunludur.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik alanı zorunludur.")
                .Must(content => content != "<p><br></p>").WithMessage("İçerik alanı zorunludur.");
        }
    }
}
