using FluentValidation;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;

namespace TechnicalService.Validations.Common.Validations.PersonnelValidators
{
    public class UpdatePersonnelValidator : AbstractValidatorBase<UpdatePersonnelDto>
    {
        public UpdatePersonnelValidator()
        {
            RuleFor(p => p.Name)
            .NotEmpty().WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(20).WithMessage("İsim en fazla 20 karakter olabilir.");

            RuleFor(p => p.Surname)
                .NotEmpty().WithMessage("Soyisim alanı boş olamaz.")
                .MaximumLength(20).WithMessage("Soyisim en fazla 20 karakter olabilir.");

            RuleFor(p => p.IdentityNumber)
                .NotEmpty().WithMessage("Kimlik numarası alanı boş olamaz.")
                .Length(11).WithMessage("Kimlik numarası 11 karakter olmalıdır.")
                .Matches("^[0-9]*$").WithMessage("Kimlik numarası sadece rakamlardan oluşmalıdır.")
                .Must(BeAValidTurkishIdentityNumber).WithMessage("Geçerli bir T.C. Kimlik Numarası giriniz.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(40).WithMessage("E-posta en fazla 40 karakter olabilir.");

            RuleFor(p => p.InternalEmail)
                .NotEmpty().WithMessage("Dahili e-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir dahili e-posta adresi giriniz.")
                .MaximumLength(40).WithMessage("Dahili e-posta en fazla 40 karakter olabilir.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası alanı boş olamaz.")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

            RuleFor(p => p.Gender)
                .NotNull().WithMessage("Cinsiyet alanı boş olamaz.")
                .IsInEnum().WithMessage("Geçersiz cinsiyet değeri.");

            RuleFor(p => p.Role)
                .NotNull().WithMessage("Rol alanı boş olamaz.")
                .IsInEnum().WithMessage("Geçersiz rol değeri.");

            RuleFor(p => p.PersonnelStatus)
                .NotNull().WithMessage("Personel durumu alanı boş olamaz.")
                .IsInEnum().WithMessage("Geçersiz personel durumu değeri.");

            RuleFor(p => p.TechnicalServices)
                .NotNull().WithMessage("Teknik servis alanı boş olamaz.");

            RuleFor(p => p.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi alanı boş olamaz.");

            RuleFor(p => p.HireDate)
                .NotEmpty().WithMessage("İşe başlama tarihi alanı boş olamaz.");

            //RuleFor(p => p.Address)
            //    .MaximumLength(200).WithMessage("Adres en fazla 200 karakter olabilir.")
            //    .When(p => !string.IsNullOrEmpty(p.Address)).WithMessage("Adres alanı boş olamaz.");

        }

        private bool BeAValidTurkishIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrEmpty(identityNumber) || identityNumber.Length != 11 || !identityNumber.All(char.IsDigit))
                return false;

            if (identityNumber[0] == '0')
                return false;

            int[] digits = new int[11];
            for (int i = 0; i < 11; i++)
                digits[i] = int.Parse(identityNumber[i].ToString());


            int sumOfOddPositionDigits = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int sumOfEvenPositionDigits = digits[1] + digits[3] + digits[5] + digits[7];

            int calculated10thDigit = ((sumOfOddPositionDigits * 7) - sumOfEvenPositionDigits) % 10;
            if (calculated10thDigit < 0)
                calculated10thDigit += 10;

            if (digits[9] != calculated10thDigit)
                return false;

            int sumOfFirst10Digits = 0;
            for (int i = 0; i < 10; i++)
                sumOfFirst10Digits += digits[i];

            int calculated11thDigit = sumOfFirst10Digits % 10;

            if (digits[10] != calculated11thDigit)
                return false;

            return true;
        }
    }
}
