using System;
using Athena.Core.Models.Identity;
using Athena.Core.Validation;
using Athena.Data.Extensions;
using Athena.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Athena.Setup
{
    public static class ServiceCollectionExtensions
    {
        public const string AUTH_GOOGLE_CLIENT_KEY = nameof(AUTH_GOOGLE_CLIENT_KEY);
        public const string AUTH_GOOGLE_CLIENT_SECRET = nameof(AUTH_GOOGLE_CLIENT_SECRET);
        
        public static IServiceCollection AddAthenaServices(this IServiceCollection services,
            IConfiguration conf)
        {
            services.AddIdentity<AthenaUser, AthenaRole>()
                .AddDefaultTokenProviders();

            services.AddAthenaRepositoriesUsingPostgres()
                .AddAthenaIdentityServices()
                .AddAuthenticationProviders()
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());
            
            return services;
        }

        private static IServiceCollection AddAuthenticationProviders(this IServiceCollection services)
        {
            var auth = services.AddAuthentication();
            
            if (!auth.TryAddGoogle())
            {
                Serilog.Log.Fatal($"Failed to add google authentication. Did you set {AUTH_GOOGLE_CLIENT_KEY} and {AUTH_GOOGLE_CLIENT_SECRET}?");
            }
            
            return services;
        }

        private static bool TryAddGoogle(this AuthenticationBuilder auth)
        {
            var clientKey = Environment.GetEnvironmentVariable(AUTH_GOOGLE_CLIENT_KEY);
            var clientSecret = Environment.GetEnvironmentVariable(AUTH_GOOGLE_CLIENT_SECRET);

            if (string.IsNullOrEmpty(clientKey) || string.IsNullOrEmpty(clientSecret))
            {
                return false;
            }

            auth.AddGoogle(g =>
            {
                g.CallbackPath = "/account/oauth/google";
                g.ClientId = clientKey;
                g.ClientSecret = clientSecret;
            });

            Serilog.Log.Information("Added google authentication with client id {clientId}", clientKey);
            return true;
        }
    }
}