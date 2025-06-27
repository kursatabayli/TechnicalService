using FluentValidation;
using TechnicalService.DTOs.DTOs.UserDTOs;

namespace TechnicalService.Validations.Common.Validations.UserValidators
{
    public class ChangeUserPassowrdValidator : AbstractValidatorBase<ChangeUserPasswordDto>
    {
        public ChangeUserPassowrdValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen mevcut şifrenizi giriniz.");

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Şifre alanı zorunludur.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                    .WithMessage("Şifre en az bir büyük harf, küçük harf, rakam ve özel karakter içermelidir.")
                .MaximumLength(20).WithMessage("Şifre 20 karakteri geçemez.");

            RuleFor(x => x.ApplyNewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen şirenizi doğrulayınız.")
                .Equal(x => x.NewPassword).WithMessage("Şifreler uyuşmuyor.");
        }
    }
}
