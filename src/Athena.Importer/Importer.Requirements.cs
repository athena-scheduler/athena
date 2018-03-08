using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportRequirements()
        {
            var root = Path.Combine(_dataPath, "requirements");
            
            if (!Directory.Exists(root))
            {
                Log.Warning("No requirements to import");
                return;
            }

            Log.Information("Importing requirements");

            foreach (var requirement in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {requirement}", requirement);
            }
        }
    }
}