using FluentValidation;
using TechnicalService.DTOs.DTOs.BrandDTOs;

namespace TechnicalService.Validations.Common.Validations.BrandValidators
{
    public class CreateBrandValidator : AbstractValidatorBase<CreateBrandDto>
    {
        public CreateBrandValidator()
        {
            RuleFor(x => x.BrandName)
                .NotEmpty().WithMessage("Marka adı boş olamaz")
                .Must(BeAValidBrandName).WithMessage("Geçersiz karakter içeriyor!");
        }

        private bool BeAValidBrandName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.All(c => char.IsLetterOrDigit(c) || c == ' ');
        }
    }
}
