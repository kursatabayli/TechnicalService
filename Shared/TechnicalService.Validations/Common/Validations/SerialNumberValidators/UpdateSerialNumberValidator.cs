using FluentValidation;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;

namespace TechnicalService.Validations.Common.Validations.SerialNumberValidators
{
    public class UpdateSerialNumberValidator : AbstractValidatorBase<UpdateSerialNumberDto>
    {
        public UpdateSerialNumberValidator()
        {
            RuleFor(x => x.Serial_Number)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Seri numarası boş olamaz.");

            RuleFor(x => x.Product)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen bir ürün seçiniz.");

            RuleFor(x => x.RegisterDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kayıt tarihi boş olamaz.");
        }
    }
}
