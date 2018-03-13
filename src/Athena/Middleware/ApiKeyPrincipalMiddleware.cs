using System;
using System.Net;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Exceptions;
using Athena.Models.Identity;
using Microsoft.AspNetCore.Http;

namespace Athena.Middleware
{
    public class ApiKeyPrincipalMiddleware
    {
        private const string ATHENA_API_HEADER_KEY = "X-ATHENA-API-KEY";
        
        private readonly RequestDelegate _next;

        public ApiKeyPrincipalMiddleware(RequestDelegate next) => 
            _next = next ?? throw new ArgumentNullException(nameof(next));
        
        public async Task InvokeAsync(HttpContext ctx, IUserApiKeyStore apiKeys)
        {
            if (ctx.Request.Headers.TryGetValue(ATHENA_API_HEADER_KEY, out var apiKey))
            {
                var user = await apiKeys.GetUserForApiKey(apiKey);

                if (user == null)
                {
                    throw new ApiException(HttpStatusCode.Forbidden, "Unknown API Key");
                }

                ctx.User = new AthenaPrincipal(ctx.User, user);
            }
            
            await _next(ctx);
        }
    }
}