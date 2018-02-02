using System;
using Athena.Data;
using Athena.Setup;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Athena
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfig.SetupSerilog();

            try
            {
                Log.Information("Ensuring database is migrated");
                new DatabaseMigrator(Athena.Data.Extensions.ServiceCollectionExtensions.ConnectionString).Migrate();

                Log.Information("Starting Web Host");
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Web Host terminated unexpectedly");
                Environment.Exit(1);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
