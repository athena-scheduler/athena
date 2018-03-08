using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Athena.Importer
{
    public partial class Importer
    {
        private async Task ImportCourses()
        {
            var root = Path.Combine(_dataPath, "courses");

            if (!Directory.Exists(root))
            {
                Log.Warning("No courses to import");
                return;
            }

            Log.Information("Importing courses");
            
            foreach (var course in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                Log.Debug("Importing {course}", course);
            }
        }
    }
}