using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using TechnicalService.DTOs.Enums;
using TechnicalService.Shared.Extensions;

namespace TechnicalService.UserUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices(config =>
                    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight);

            builder.Services.AuthServices();
            builder.Services.RegisterServices();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(nameof(AppPolicies.UserAccesses), policy => policy.RequireRole(RoleDto.User.GetDescription()));
            });



            await builder.Build().RunAsync();
        }
    }
}
