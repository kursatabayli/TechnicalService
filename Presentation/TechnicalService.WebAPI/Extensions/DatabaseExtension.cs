using Microsoft.EntityFrameworkCore;
using TechnicalService.Persistence.Context;

namespace TechnicalService.WebAPI.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 29)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

            return services;
        }

        //public static IServiceCollection _AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<AppDbContext>(options =>
        //        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
        //        sqlOptions => sqlOptions.EnableRetryOnFailure()
        //        ));

        //    return services;
        //}
    }
}