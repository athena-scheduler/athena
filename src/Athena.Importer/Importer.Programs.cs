using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportPrograms()
        {
            var root = Path.Combine(_dataPath, "programs");

            if (!Directory.Exists(root))
            {
                Log.Warning("No programs to import");
                return;
            }

            Log.Information("Importing programs");

            foreach (var program in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {program}", program);
            }
        }
    }
}