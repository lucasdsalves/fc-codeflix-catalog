using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services)
        {
            services.AddDbConnection();

            return services;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services)
        {
            services.AddDbContext<CodeflixCatalogDbContext>(options =>
                options.UseInMemoryDatabase("InMemory-DSV-Database")
            );

            return services;
        }
    }
}
