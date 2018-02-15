using Athena.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;

namespace Athena.Setup
{
    public static class LoggingConfig
    {
        public static void SetupSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .DestructAthenaTypes()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }

        private static LoggerConfiguration DestructAthenaTypes(this LoggerConfiguration c) => c
            .Destructure.ByTransforming<AthenaUser>(u => new {u.Id, u.UserName, u.Email})
            .Destructure.ByTransforming<AthenaRole>(r => new {r.Id, r.Name})
            .Destructure.ByTransforming<ExternalLoginInfo>(i => new {i.LoginProvider, user = i.Principal.Identity.Name});
    }
}