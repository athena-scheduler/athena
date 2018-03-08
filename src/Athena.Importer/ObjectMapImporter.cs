using System;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public class ObjectMapImporter : AbstractImporter
    {
        public ObjectMapImporter(Uri apiEndpoint) : base(apiEndpoint)
        {
        }

        public override async Task Import()
        {
            Log.Information("Linking Objects...");
        }
    }
}