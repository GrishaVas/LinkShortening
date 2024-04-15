using LinkShortening.Infrastructure.Abstractions;
using LinkShortening.Infrastructure.Implemintations;

namespace LinkShortening.MVC.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static void AddRepositoriesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ILinksRepository, LinksRepository>();
        }
    }
}
