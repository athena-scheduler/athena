using System;
using System.IO;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Importer.Provider;
using McMaster.Extensions.CommandLineUtils;
using Serilog;
using Serilog.Core;

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

            if (!Uri.TryCreate(ApiEndpoint, UriKind.Absolute, out var uri))
            {
                app.ShowHelp();
                return 1;
            }

            if (!Directory.Exists(DataPath))
            {
                Log.Fatal("Could not find all or part of the path {dataPath}", DataPath);
                return 1;
            }

            try
            {
                await new GenericImporter<Campus>(uri, new JsonFilesystemDataProvider<Campus>(DataPath)).Import();
                await new GenericImporter<Institution>(uri, new JsonFilesystemDataProvider<Institution>(DataPath)).Import();
                await new GenericImporter<Course>(uri, new JsonFilesystemDataProvider<Course>(DataPath)).Import();
                await new GenericImporter<Offering>(uri, new JsonFilesystemDataProvider<Offering>(DataPath)).Import();
                await new GenericImporter<Meeting>(uri, new JsonFilesystemDataProvider<Meeting>(DataPath)).Import();
                await new GenericImporter<Core.Models.Program>(uri, new JsonFilesystemDataProvider<Core.Models.Program>(DataPath)).Import();
                await new GenericImporter<Requirement>(uri, new JsonFilesystemDataProvider<Requirement>(DataPath)).Import();

                await new ObjectMapImporter(uri).Import();
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
