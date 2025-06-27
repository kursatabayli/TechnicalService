using TechnicalService.Persistence.Repositories;
using TechnicalService.Persistence.Repositories.ProductRepositories;
using TechnicalService.Persistence.Repositories.SerialNumberRepositories;
using TechnicalService.Persistence.Repositories.UserProductRepositories;
using TechnicalService.Persistence.Services;
using System.Collections.Concurrent;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Persistence.Helpers.Implementations;
using TechnicalService.Persistence.Helpers.Contracts;
using TechnicalService.Persistence.Repositories.PersonnelRepositories;
using TechnicalService.Persistence.Repositories.ServiceRecordRepositories;

namespace TechnicalService.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            services.AddScoped<IUserProductRepository, UserProductRepository>();
            services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
            services.AddScoped<IServiceRecordStepsRepository, ServiceRecordStepsRepository>();
            services.AddScoped<IPersonnelRepository, PersonnelRepository>();

            //Services
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IAuthService, AuthService>();
            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ConcurrentDictionary<Type, object>>();

            //builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

            // IEmailService'i kaydet
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITemplateHelper, TemplateHelper>();
            services.AddScoped<ISmsService, SmsService>();
            return services;
        }
    }
}
