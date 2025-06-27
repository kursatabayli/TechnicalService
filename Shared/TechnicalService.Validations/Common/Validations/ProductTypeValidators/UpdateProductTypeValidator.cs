using FluentValidation;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;

namespace TechnicalService.Validations.Common.Validations.ProductTypeValidators
{
    public class UpdateProductTypeValidator : AbstractValidatorBase<UpdateProductTypeDto>
    {
        public UpdateProductTypeValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Ürün tipi adı boş olamaz.")
                .MaximumLength(20).WithMessage("Ürün tipi adı en fazla 20 karakter olabilir.");
        }
    }
} 