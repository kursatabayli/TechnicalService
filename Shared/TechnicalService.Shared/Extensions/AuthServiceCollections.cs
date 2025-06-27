using Microsoft.AspNetCore.Components.Authorization;
using TechnicalService.Shared.AuthHelpers;
using TechnicalService.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Shared.Extensions
{
    public static class AuthServiceCollections
    {
        public static IServiceCollection AuthServices(this IServiceCollection services)
        {
            services.AddScoped<CustomAuthStateProvider>();
            //services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientTypes.AuthClient));
            //services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientTypes.PublicClient));
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            services.AddTransient<TokenRefreshHandler>();

            services.AddHttpClient(ClientTypes.PersonnelAuthClient, client =>
            {
                client.BaseAddress = new Uri(Endpoints.BaseUrl);
                client.DefaultRequestHeaders.Add(HeaderTypes.HeaderKey.GetDescription(), nameof(HeaderTypes.Personnel));

            }).AddHttpMessageHandler<TokenRefreshHandler>();

            services.AddHttpClient(ClientTypes.PersonnelPublicClient, client =>
            {
                client.BaseAddress = new Uri(Endpoints.BaseUrl);
                client.DefaultRequestHeaders.Add(HeaderTypes.HeaderKey.GetDescription(), nameof(HeaderTypes.Personnel));
            });

            services.AddHttpClient(ClientTypes.UserAuthClient, client =>
            {
                client.BaseAddress = new Uri(Endpoints.BaseUrl);
                client.DefaultRequestHeaders.Add(HeaderTypes.HeaderKey.GetDescription(), nameof(HeaderTypes.User));
            }).AddHttpMessageHandler<TokenRefreshHandler>();

            services.AddHttpClient(ClientTypes.UserPublicClient, client =>
            {
                client.BaseAddress = new Uri(Endpoints.BaseUrl);
                client.DefaultRequestHeaders.Add(HeaderTypes.HeaderKey.GetDescription(), nameof(HeaderTypes.User));
            });


            return services;
        }
    }
}
