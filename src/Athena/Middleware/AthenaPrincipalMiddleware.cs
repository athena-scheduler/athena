using System;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Extensions;
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
            ctx.User = await ctx.User.TryGetAthenaPrincipal(userManager);
            
            await _next(ctx);
        }
    }
}