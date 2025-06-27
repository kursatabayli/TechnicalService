using FluentValidation;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;

namespace TechnicalService.Validations.Common.Validations.ServiceRecordValidators
{
    public class CreateServiceRecordValidator : AbstractValidatorBase<CreateServiceRecordDto>
    {
        public CreateServiceRecordValidator()
        {
            RuleFor(x => x.UserProduct)
                .NotEmpty()
                .WithMessage("Lütfen onarım istediğiniz ürünü seçin.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Başlık boş olamaz.")
                .MaximumLength(100)
                .WithMessage("Başlık en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Açıklama boş olamaz.")
                .MaximumLength(500)
                .WithMessage("Açıklama en fazla 500 karakter olmalıdır.");
        }
    }
}
