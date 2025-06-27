using FluentValidation;
using TechnicalService.DTOs.DTOs.UserProductDTOs;

namespace TechnicalService.Validations.Common.Validations.UserProductValidators
{
    public class AddUserProductsValidator : AbstractValidatorBase<AddUserProductDto>
    {
        public AddUserProductsValidator()
        {
            RuleFor(x => x.SerialNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen ürününüze ait seri numarasını giriniz");

            RuleFor(x=>x.PurchaseDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen ürünün satın alındığı tarihi giriniz");
        }
    }
}
