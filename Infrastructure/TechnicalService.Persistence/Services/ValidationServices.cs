using TechnicalService.Application.Features.CQRS.Brands.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechnicalService.Validations.Common.Validations.BrandValidators;
using TechnicalService.Validations.Common.Validations.ProductValidators;
using TechnicalService.Validations.Common.Behaviors;

namespace TechnicalService.Persistence.Services
{
    public static class ValidationServices
    {
        public static IServiceCollection AddValitationervices(this IServiceCollection services)
        {

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBrandCommand).Assembly));
            //services.AddValidatorsFromAssembly(typeof(CreateBrandValidator).Assembly);
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateBrandValidator).Assembly));
            //services.AddValidatorsFromAssembly(typeof(UpdateBrandValidator).Assembly);

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductValidator).Assembly));
            //services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateProductValidator).Assembly));
            //services.AddValidatorsFromAssembly(typeof(UpdateProductValidator).Assembly);


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
