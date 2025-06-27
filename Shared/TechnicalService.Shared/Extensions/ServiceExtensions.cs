using Microsoft.Extensions.DependencyInjection;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Shared.Services.Implementations;


namespace TechnicalService.Shared.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDataService<>), typeof(DataService<>));
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IUIStateService, UIStateService>();
            services.AddScoped<IDeleteDialogService, DeleteDialogService>();

            services.AddLocalization();
            return services;
        }
    }
}
