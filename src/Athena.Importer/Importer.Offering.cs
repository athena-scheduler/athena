using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportOfferings()
        {
            var root = Path.Combine(_dataPath, "offerings");

            if (!Directory.Exists(root))
            {
                Log.Warning("No offerings to import");
                return;
            }

            Log.Information("Importing offerings");

            foreach (var offering in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {offering}", offering);
            }
        }
    }
}