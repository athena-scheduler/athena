using System;
using System.Threading.Tasks;
using Athena.Core.Models.Identity;
using Athena.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StackExchange.Profiling;

namespace Athena.Middleware
{
    public class AthenaPrincipalMiddleware
    {
        private readonly RequestDelegate _next;

        public AthenaPrincipalMiddleware(RequestDelegate next) => 
            _next = next ?? throw new ArgumentNullException(nameof(next));
        
        public async Task InvokeAsync(HttpContext ctx, UserManager<AthenaUser> userManager)
        {
            using (var step = MiniProfiler.Current.Step("GetAthenaPrincipal"))
            {
                ctx.User = await ctx.User.TryGetAthenaPrincipal(userManager);
            }
            
            await _next(ctx);
        }
    }
}