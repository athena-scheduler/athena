using System;
using System.IO;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private readonly string _apiEndpoint;
        private readonly string _dataPath;

        public Importer(Uri apiEndpoint, string dataPath)
        {
            var baseUrl = apiEndpoint?.ToString() ?? throw new ArgumentNullException(nameof(apiEndpoint));

            _apiEndpoint = baseUrl.TrimEnd('/').EndsWith("api") ? baseUrl : $"{baseUrl}/api";
            _dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));

            if (!Directory.Exists(_dataPath))
            {
                throw new ArgumentException("Could not find all or part of the specified import path", nameof(dataPath));
            }
        }

        private IFlurlRequest CreateRequest() => _apiEndpoint
            .AppendPathSegment("v1")
            .WithTimeout(TimeSpan.FromSeconds(30));

        public async Task Import()
        {
            Log.Information("Importing Data from {dataPath} to {api}", _dataPath, _apiEndpoint);
            
            await ImportCampuses();
            await ImportInstitutions();
            await ImportCourses();
            await ImportOfferings();
            await ImportMeetings();
            await ImportPrograms();
            await ImportRequirements();

            await LinkAllTheThings();
        }
    }
}