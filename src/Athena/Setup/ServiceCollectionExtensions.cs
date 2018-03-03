using System;
using Athena.Core.Models.Identity;
using Athena.Core.Validation;
using Athena.Data.Extensions;
using Athena.Handlers;
using Athena.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/_profile";

                options.UserIdProvider = r =>
                    r.HttpContext.User?.ToAthenaUser()?.Id.ToString() ?? Guid.NewGuid().ToString();
            });
            
            services.AddAthenaRepositoriesUsingPostgres()
                .AddAthenaIdentityServices()
                .AddAuthenticationProviders()
                .AddAuthorization()
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());
            
            return services;
        }

        private static IServiceCollection AddAuthenticationProviders(this IServiceCollection services)
        {
            services.AddTransient<ApiKeyHandler>();
            
            var auth = services.AddAuthentication();
            auth.AddScheme<ApiKeyHandler._, ApiKeyHandler>(ApiKeyHandler.SCHEME, ApiKeyHandler.SCHEME, null);

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