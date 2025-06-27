using FluentValidation;
using TechnicalService.DTOs.DTOs.AuthDTOs;

namespace TechnicalService.Validations.Common.Validations.AuthValidators
{
    public class LoginValidator : AbstractValidatorBase<LoginDto>
    {
        public LoginValidator()
        {

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-posta alanı zorunludur.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Şifre alanı zorunludur.");

        }
    }
}
