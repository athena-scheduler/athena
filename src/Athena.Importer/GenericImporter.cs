using System;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Importer.Provider;
using Flurl.Http;
using Serilog;

namespace Athena.Importer
{
    public class GenericImporter<T> : AbstractImporter where T : IUniqueObject<Guid>
    {
        protected readonly IDataProvider<T> _data;

        public GenericImporter(Uri apiEndpoint, string apiKey, IDataProvider<T> data) : base(apiEndpoint, apiKey) =>
            _data = data ?? throw new ArgumentNullException(nameof(data));

        public override async Task Import()
        {
            var typeName = typeof(T).Name.ToLowerInvariant();
            
            Log.Information("Importing objects for type {T}", typeName);

            foreach (var obj in _data.GetData())
            {
                Log.Debug("Importing {@obj}", obj);
                await CreateRequest()
                    .AppendPathSegment(typeName)
                    .PostJsonAsync(obj);
            }
        }
    }
}