using System;
using System.Net;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Exceptions;
using Athena.Extensions;
using Athena.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StackExchange.Profiling;

namespace Athena.Middleware
{
    public class ApiAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiAuthenticationMiddleware(RequestDelegate next) =>
            _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task Invoke(HttpContext ctx, UserManager<AthenaUser> userManager)
        {
            using (var step = MiniProfiler.Current.Step("ApiAuthentication"))
            {
                var result = await ctx.AuthenticateAsync(
                    ctx.Request.Headers.ContainsKey(ApiKeyHandler.ATHENA_API_HEADER_KEY) ? ApiKeyHandler.SCHEME : null
                );
                                 
                if (result.Succeeded)
                {
                    ctx.User = await result.Principal.TryGetAthenaPrincipal(userManager);
                }
                else
                {
                    throw new ApiException(HttpStatusCode.Forbidden, result.Failure?.Message ?? "Not Authenticated");
                }
            }

            await _next(ctx);
        }
    }
}