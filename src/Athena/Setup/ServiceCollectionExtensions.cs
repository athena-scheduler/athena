using Athena.Core.Validation;
using Athena.Data.Extensions;
using FluentValidation.AspNetCore;
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
                .AddMvc(options => { })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());
            
            return services;
        }
    }
}