using System;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Athena.Importer
{
    public abstract class AbstractImporter
    {
        private readonly string _apiEndpoint;
        private readonly string _apiKey;

        protected AbstractImporter(Uri apiEndpoint, string apiKey)
        {
            var baseUrl = apiEndpoint?.ToString() ?? throw new ArgumentNullException(nameof(apiEndpoint));

            _apiEndpoint = baseUrl.TrimEnd('/').EndsWith("api") ? baseUrl : $"{baseUrl}/api";
            _apiKey = apiKey;
        }

        protected IFlurlRequest CreateRequest()
        {
            var req = _apiEndpoint
                .AppendPathSegment("v1")
                .AllowHttpStatus(HttpStatusCode.Conflict, HttpStatusCode.OK)
                .WithTimeout(TimeSpan.FromSeconds(30));

            return string.IsNullOrEmpty(_apiKey) ? req : req.WithHeader("X-ATHENA-API-KEY", _apiKey);
        }

        public abstract Task Import();
    }
}