using LinkShortening.Services.Abstractions;
using LinkShortening.Services.Implemintations;

namespace LinkShortening.MVC.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddServicesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ILinksService, LinksService>();
        }
    }
}
