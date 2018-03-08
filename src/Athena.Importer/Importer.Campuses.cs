using System.IO;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportCampuses()
        {
            var root = Path.Combine(_dataPath, "campuses");

            if (!Directory.Exists(root))
            {
                Log.Warning("No campuses to import");
                return;
            }

            Log.Information("Importing Campuses");

            foreach (var campus in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {campus}", campus);
                await CreateRequest()
                    .AppendPathSegment("campus")
                    .PostJsonAsync(JsonConvert.DeserializeObject(campus));
            }
        }
    }
}