using FluentValidation;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;

namespace TechnicalService.Validations.Common.Validations.ServiceRecordValidators
{
    public class AddServiceRecordStepValidator : AbstractValidatorBase<AddServiceRecordStepDto>
    {

        public AddServiceRecordStepValidator() {
            RuleFor(x => x.StepTitle)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(50).WithMessage("Başlık 100 karakterden uzun olamaz.");
            RuleFor(x => x.StepDescription)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.");
        }
    }
}
