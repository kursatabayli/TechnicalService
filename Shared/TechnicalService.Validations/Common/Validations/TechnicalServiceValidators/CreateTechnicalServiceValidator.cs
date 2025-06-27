using FluentValidation;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;

namespace TechnicalService.Validations.Common.Validations.TechnicalServiceValidators
{
    public class CreateTechnicalServiceValidator : AbstractValidatorBase<CreateTechnicalServiceDto>
    {
        public CreateTechnicalServiceValidator()
        {
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Servis adı zorunludur.")
                .MaximumLength(100).WithMessage("Servis adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Lat)
                .NotEmpty().WithMessage("Enlem zorunludur.")
                .InclusiveBetween(-90, 90).WithMessage("Enlem -90 ile 90 arasında olmalıdır.");

            RuleFor(x => x.Lng)
                .NotEmpty().WithMessage("Boylam zorunludur.")
                .InclusiveBetween(-180, 180).WithMessage("Boylam -180 ile 180 arasında olmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres zorunludur.")
                .MaximumLength(200).WithMessage("Adres 200 karakterden uzun olamaz.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir zorunludur.")
                .MaximumLength(20).WithMessage("Şehir 20 karakterden uzun olamaz.");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("İlçe zorunludur.")
                .MaximumLength(50).WithMessage("İlçe 50 karakterden uzun olamaz.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Posta kodu zorunludur.")
                .Matches(@"^\d{5}$").WithMessage("Posta kodu 5 haneli bir sayı olmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefon numarası geçerli bir uluslararası formatta olmalıdır.");
        }
    }
}
