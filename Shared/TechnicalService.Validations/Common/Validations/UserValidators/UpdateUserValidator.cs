using FluentValidation;
using TechnicalService.DTOs.DTOs.UserDTOs;

namespace TechnicalService.Validations.Common.Validations.UserValidators
{
    public class UpdateUserValidator : AbstractValidatorBase<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ad alanı zorunludur.")
                .MaximumLength(50).WithMessage("Ad 50 karakteri geçemez.");

            RuleFor(x => x.Surname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Soyadı alanı zorunludur.")
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
        }
    }
}
