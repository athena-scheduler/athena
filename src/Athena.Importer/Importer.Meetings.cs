using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportMeetings()
        {
            var root = Path.Combine(_dataPath, "meetings");

            if (!Directory.Exists(root))
            {
                Log.Warning("No meetings to import");
                return;
            }

            Log.Information("Importing meetings");

            foreach (var meeting in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {meeting}", meeting);
            }
        }
    }
}