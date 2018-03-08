using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Serilog;

namespace Athena.Importer
{
    [HelpOption]
    public class Program
    {
        [Option(Description = "Athena API Endpoint", LongName = "api-endpoint", ShortName = "a")]
        public string ApiEndpoint { get; } = null;

        [Option(Description = "Path to sample data", LongName = "data-path", ShortName = "d")]
        public string DataPath { get; } = "/athena/importer/data";

        public static async Task<int> Main(string[] args) => await CommandLineApplication.ExecuteAsync<Program>(args);
        
        private async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();
            
            if (string.IsNullOrEmpty(ApiEndpoint))
            {
                app.ShowHelp();
                return 1;
            }

            if (!Uri.TryCreate(ApiEndpoint, UriKind.Absolute, out var url))
            {
                app.ShowHelp();
                return 1;
            }

            try
            {
                await new Importer(url, DataPath).Import();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Import Failed");
                return 1;
            }
            
            Log.Information("Import Complete");

            return 0;
        }
    }
}
