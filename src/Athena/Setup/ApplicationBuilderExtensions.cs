using System;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Core.Models.Identity;
using Athena.Core.Repositories;
using Athena.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

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
            app.UseMiniProfiler();
            
            app.UseWhen(ctx => ctx.Request.Path.HasValue && ctx.Request.Path.StartsWithSegments("/api"), builder =>
            {
                builder.UseMiddleware<ApiAuthenticationMiddleware>();
            });
        
            app.UseWhen(ctx => !ctx.Request.Path.HasValue || !ctx.Request.Path.StartsWithSegments("/api"), builder =>
            {
                builder.UseMiddleware<AthenaPrincipalMiddleware>();
            });
            
            app.UseMvc();

            Task.Run(async () =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
                using (var userRoles = scope.ServiceProvider.GetRequiredService<IUserRoleStore<AthenaUser>>())
                {
                    if ((await userRoles.GetUsersInRoleAsync(AthenaRole.NormalizedAdminRoleName, CancellationToken.None)).Count == 0)
                    {
                        var id = Guid.NewGuid();
                        var key = Environment.GetEnvironmentVariable("ATHENA_ADMIN_API_KEY") ?? Guid.NewGuid().ToString();

                        Log.Debug("Attempting to create default admin user");
                        
                        var user = new AthenaUser()
                        {
                            Id = id,
                            Student = new Student()
                            {
                                Id = id,
                                Name = "Athena Administrator",
                                Email = "athena@localhost"
                            },
                            ApiKey = key,
                            UserName = "athena",
                            NormalizedUserName = "ATHENA",
                            Email = "athena@localhost",
                            NormalizedEmail = "ATHENA@LOCALHOST"
                        };

                        var students = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
                        await students.AddAsync(user.Student);

                        await userRoles.CreateAsync(user, CancellationToken.None);
                        await userRoles.AddToRoleAsync(user, AthenaRole.NormalizedAdminRoleName, CancellationToken.None);
                        
                        Log.Information("Created default admin API Key {key}", key);
                    }
                }
            }, CancellationToken.None).GetAwaiter().GetResult();
            
            return app;
        }
    }
}