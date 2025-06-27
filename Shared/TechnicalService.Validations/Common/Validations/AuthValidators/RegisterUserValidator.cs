using FluentValidation;
using TechnicalService.DTOs.DTOs.AuthDTOs;

namespace TechnicalService.Validations.Common.Validations.AuthValidators
{
    public class RegisterUserValidator : AbstractValidatorBase<RegisterDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ad alanı zorunludur.")
                .MaximumLength(50).WithMessage("Ad 50 karakteri geçemez.");

            RuleFor(x => x.Surname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Soyad alanı zorunludur.")
                .MaximumLength(50).WithMessage("Soyad 50 karakteri geçemez.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-posta alanı zorunludur.")
                .EmailAddress().WithMessage("Geçersiz e-posta formatı.")
                .MaximumLength(100).WithMessage("E-posta çok uzun.");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Matches(@"^5[0-9]{9}$").WithMessage("Geçersiz telefon formatı. 5 ile başlamalı ve 10 haneli olmalıdır.")
                .MaximumLength(10).WithMessage("Telefon numarası 10 haneden uzun olamaz.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Şifre alanı zorunludur.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                    .WithMessage("Şifre en az bir büyük harf, küçük harf, rakam ve özel karakter içermelidir.")
                .MaximumLength(20).WithMessage("Şifre 20 karakteri geçemez.");

            RuleFor(x => x.ApplyPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lütfen şirenizi doğrulayınız.")
                .Equal(x => x.Password).WithMessage("Şifreler uyuşmuyor.");
        }
    }
}
