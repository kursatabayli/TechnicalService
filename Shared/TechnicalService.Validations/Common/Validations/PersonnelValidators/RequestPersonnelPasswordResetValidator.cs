using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;

namespace TechnicalService.Validations.Common.Validations.PersonnelValidators
{
    public class RequestPersonnelPasswordResetValidator : AbstractValidatorBase<PersonnelRequestPasswordResetLinkDto>
    {
        public RequestPersonnelPasswordResetValidator()
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
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.InternalEmail)).WithMessage("Geçerli bir dahili e-posta adresi giriniz.")
                .MaximumLength(40).WithMessage("Dahili e-posta en fazla 40 karakter olabilir.");

            RuleFor(p => p.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi alanı boş olamaz.");
        }

        private bool BeAValidTurkishIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrEmpty(identityNumber) || identityNumber.Length != 11 || !identityNumber.All(char.IsDigit))
                return false;

            if (identityNumber.StartsWith("0"))
                return false;

            int[] digits = identityNumber.Select(c => int.Parse(c.ToString())).ToArray();

            int sumOdd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int sumEven = digits[1] + digits[3] + digits[5] + digits[7];

            int checkDigit10 = ((sumOdd * 7) - sumEven) % 10;
            if (checkDigit10 < 0) checkDigit10 += 10;

            if (digits[9] != checkDigit10)
                return false;

            int totalSum = 0;
            for (int i = 0; i < 10; i++)
            {
                totalSum += digits[i];
            }

            int checkDigit11 = totalSum % 10;
            if (digits[10] != checkDigit11)
                return false;

            return true;
        }
    }
}
