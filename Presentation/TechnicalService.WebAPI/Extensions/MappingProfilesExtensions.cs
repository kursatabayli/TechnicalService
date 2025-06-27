using TechnicalService.Application.Mappings;

namespace TechnicalService.WebAPI.Extensions
{
    public static class MappingProfilesExtensions
    {
        public static IServiceCollection RegisterMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfiles(
                [
                    new AuthMapping(),
                    new BrandMapping(),
                    new ProductMapping(),
                    new ProductTypeMapping(),
                    new SerialNumberMapping(),
                    new UserMapping(),
                    new UserProductMapping()
                ]);
            });

            return services;
        }
    }
}