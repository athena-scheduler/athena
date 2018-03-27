using System.Data;
using Athena.Core.Models.Identity;
using Athena.Core.Repositories;
using Athena.Data.Repositories;
using Athena.Data.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Athena.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAthenaRepositoriesUsingPostgres(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringProvider, EnvironmentVariableConnectionStringProvider>();
            services.AddScoped<IDbConnection>(s => new ProfiledDbConnection(new NpgsqlConnection(s.GetRequiredService<IConnectionStringProvider>().GetConnectionString()), MiniProfiler.Current));
            
            services.AddScoped<ICampusRepository, CampusRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IOfferingReository, OfferingRepository>();
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            return services;
        }

        public static IServiceCollection AddAthenaIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IUserStore<AthenaUser>, AthenaUserStore>();
            services.AddTransient<IUserRoleStore<AthenaUser>, AthenaUserStore>();
            services.AddTransient<IUserApiKeyStore, AthenaUserStore>();
            services.AddTransient<IRoleStore<AthenaRole>, AthenaRoleStore>();

            return services;
        }
    }
}