using System;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Handlers;
using Athena.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Athena.Middleware
{
    public class AthenaPrincipalMiddleware
    {
        private readonly RequestDelegate _next;

        public AthenaPrincipalMiddleware(RequestDelegate next) => 
            _next = next ?? throw new ArgumentNullException(nameof(next));
        
        public async Task InvokeAsync(HttpContext ctx, UserManager<AthenaUser> userManager)
        {
            var athenaUser = await (userManager ?? throw new ArgumentNullException(nameof(userManager)))
                .GetUserAsync(ctx.User);

            if (athenaUser != null)
            {
                ctx.User = new AthenaPrincipal(ctx.User, athenaUser);
            }
            
            await _next(ctx);
        }
    }
}