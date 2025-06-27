using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using TechnicalService.DTOs.Enums;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Extensions;

namespace TechnicalService.Portal
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
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(nameof(AppPolicies.AdminOnly), policy => policy.RequireRole(RoleDto.Admin.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.ManagerAccesses), policy => policy.RequireRole(RoleDto.Manager.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.TechnicianAccesses), policy => policy.RequireRole(RoleDto.Technician.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.CustomerServiceAccesses), policy => policy.RequireRole(RoleDto.CustomerService.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.ManagementAccess), policy => policy.RequireRole(RoleDto.Admin.GetDescription(), RoleDto.Manager.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.OperationalStaff), policy => policy.RequireRole(
                    RoleDto.Manager.GetDescription(), 
                    RoleDto.Technician.GetDescription(), 
                    RoleDto.CustomerService.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.AllEmployees), policy => policy.RequireRole(
                    RoleDto.Admin.GetDescription(), 
                    RoleDto.Manager.GetDescription(), 
                    RoleDto.Technician.GetDescription(), 
                    RoleDto.CustomerService.GetDescription()));
            });
            builder.Services.RegisterServices();


            await builder.Build().RunAsync();
        }
    }
}
