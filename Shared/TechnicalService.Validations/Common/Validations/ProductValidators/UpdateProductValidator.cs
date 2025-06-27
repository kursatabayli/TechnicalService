using FluentValidation;
using TechnicalService.DTOs.DTOs.ProductDTOs;

namespace TechnicalService.Validations.Common.Validations.ProductValidators
{
    public class UpdateProductValidator : AbstractValidatorBase<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Ürün adı boş olamaz")
                .Must(BeAValidProductName).WithMessage("Geçersiz karakter içeriyor!");

            RuleFor(x => x.Brand)
                .NotNull().WithMessage("Lütfen bir marka seçiniz!");

            RuleFor(x => x.ProductType)
                .NotNull().WithMessage("Lütfen bir ürün türü seçiniz!");
        }

        private bool BeAValidProductName(string name) => !string.IsNullOrEmpty(name) && name.All(c => char.IsLetterOrDigit(c) || c == ' ');
    }
}
