using Athena.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Setup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAthenaServices(this IServiceCollection services,
            IConfiguration conf)
        {
            services.AddAthenaRepositoriesUsingPostgres()
                    .AddMvc(options =>
                    {
                    });
            
            return services;
        }
    }
}