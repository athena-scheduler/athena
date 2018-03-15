﻿using System;
using System.Data;
using Athena.Core.Models.Identity;
using Athena.Core.Repositories;
using Athena.Data.Repositories;
using Athena.Data.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Athena.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static readonly string DEFAULT_CONNECTION_STRING = "Server=localhost;User ID=postgres;Database=postgres";
        public static readonly string CONNECTION_STRING_ENV = "ATHENA_CONNECTION_STRING";
        
        private static readonly Lazy<string> _connectionString = new Lazy<string>(() =>
            Environment.GetEnvironmentVariable(CONNECTION_STRING_ENV) ?? DEFAULT_CONNECTION_STRING
        );
        
        public static string ConnectionString => _connectionString.Value;
        
        public static IServiceCollection AddAthenaRepositoriesUsingPostgres(this IServiceCollection services)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ArgumentException("Connection string required", nameof(ConnectionString));
            }
            
            services.AddScoped<IDbConnection>(s => new NpgsqlConnection(ConnectionString));
            
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