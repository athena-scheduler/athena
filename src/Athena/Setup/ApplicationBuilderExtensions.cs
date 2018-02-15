using Athena.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;

namespace Athena.Setup
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAthena(this IApplicationBuilder app, IHostingEnvironment env,
            IConfiguration configuration)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<SerilogHttpMiddleware>();
            app.UseMiddleware<CustomErrorHandlerMiddleware>();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
            
            return app;
        }
    }
}