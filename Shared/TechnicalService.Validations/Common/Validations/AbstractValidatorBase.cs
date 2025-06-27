using FluentValidation;

namespace TechnicalService.Validations.Common.Validations
{
    public abstract class AbstractValidatorBase<T> : AbstractValidator<T>
    {
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var context = ValidationContext<T>.CreateWithOptions(
                    (T)model,
                    opts => opts.IncludeProperties(propertyName)
                );

                var result = await ValidateAsync(context);

                return result.IsValid
                    ? Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
