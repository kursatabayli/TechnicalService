using TechnicalService.Application.Services;
using TechnicalService.WebAPI.Extensions;
using TechnicalService.Persistence.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TechnicalService.Persistence.Context;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.AddServiceDefaults();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddOpenApi();

            builder.Services.AddApplicationServices();
            builder.Services.RegisterServices();
            builder.Services.RegisterMappingProfiles();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddLocalization();

            builder.Services.AddDbContext<AppDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 41)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

            builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

            var jwtSection = builder.Configuration.GetSection("JwtSection").Get<JwtSection>();
            var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection.Issuer,
                    ValidAudience = jwtSection.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string token = null;
                        var clientTypeHeader = context.Request.Headers[HeaderTypes.HeaderKey.GetDescription()].FirstOrDefault();

                        switch (clientTypeHeader)
                        {
                            case nameof(HeaderTypes.Personnel):
                                context.Request.Cookies.TryGetValue(nameof(TokenTypes.PersonnelAccessToken), out token);
                                break;
                            case nameof(HeaderTypes.User):
                                context.Request.Cookies.TryGetValue(nameof(TokenTypes.UserAccessToken), out token);
                                break;
                            default:
                                if (context.Request.Cookies.TryGetValue(nameof(TokenTypes.UserAccessToken), out var userTokenValue))
                                    token = userTokenValue;
                                else if (context.Request.Cookies.TryGetValue(nameof(TokenTypes.PersonnelAccessToken), out var personnelTokenValue))
                                    token = personnelTokenValue;
                                break;
                        }


                        context.Token = token; 
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(AppPolicies.AdminOnly), policy =>
                        policy.RequireRole(Role.Admin.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.UserAccesses), policy =>
                    policy.RequireRole(Role.User.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.ManagerAccesses), policy =>
                    policy.RequireRole(Role.Manager.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.TechnicianAccesses), policy =>
                    policy.RequireRole(Role.Technician.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.CustomerServiceAccesses), policy =>
                    policy.RequireRole(Role.CustomerService.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.ManagementAccess), policy =>
                    policy.RequireRole(Role.Admin.GetDescription(), Role.Manager.GetDescription()));

                options.AddPolicy(nameof(AppPolicies.OperationalStaff), policy =>
                    policy.RequireRole(
                        Role.Technician.GetDescription(),
                        Role.CustomerService.GetDescription()
                    ));

                options.AddPolicy(nameof(AppPolicies.AllEmployees), policy =>
                    policy.RequireRole(
                        Role.Admin.GetDescription(),
                        Role.Manager.GetDescription(),
                        Role.Technician.GetDescription(),
                        Role.CustomerService.GetDescription()
                    ));

            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorWasm",
                    policyBuilder =>
                    {
                        policyBuilder.WithOrigins(origins ?? [])
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .AllowCredentials()
                                     .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                    });
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });

            var app = builder.Build();

            //app.MapDefaultEndpoints();


            app.UseSwaggerDocumentation(app.Environment);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowBlazorWasm");
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
