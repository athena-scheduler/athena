using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Athena.Importer
{
    public abstract class AbstractImporter
    {
        private readonly string _apiEndpoint;

        protected AbstractImporter(Uri apiEndpoint)
        {
            var baseUrl = apiEndpoint?.ToString() ?? throw new ArgumentNullException(nameof(apiEndpoint));

            _apiEndpoint = baseUrl.TrimEnd('/').EndsWith("api") ? baseUrl : $"{baseUrl}/api";
        }

        protected IFlurlRequest CreateRequest() => _apiEndpoint
            .AppendPathSegment("v1")
            .WithTimeout(TimeSpan.FromSeconds(30));

        public abstract Task Import();
    }
}