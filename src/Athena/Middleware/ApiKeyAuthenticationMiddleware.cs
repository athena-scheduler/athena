using System;
using System.Net;
using System.Threading.Tasks;
using Athena.Exceptions;
using Athena.Handlers;
using Athena.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Athena.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next) =>
            _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task Invoke(HttpContext ctx)
        {
            var result = await ctx.AuthenticateAsync(ApiKeyHandler.SCHEME);

            if (result.Succeeded && result.Principal is AthenaPrincipal principal)
            {
                ctx.User = principal;
            }
            else
            {
                throw new ApiException(HttpStatusCode.Forbidden, result.Failure.Message);
            }

            await _next(ctx);
        }
    }
}