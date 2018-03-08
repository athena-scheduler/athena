using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportInstitutions()
        {
            var root = Path.Combine(_dataPath, "institutions");

            if (!Directory.Exists(root))
            {
                Log.Warning("No institutions to import");
                return;
            }

            Log.Information("Importing Institutions");

            foreach (var institution in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {instituttion}", institution);
            }
        }
    }
}