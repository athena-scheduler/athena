using System;
using System.Collections.Generic;
using System.IO;
using Athena.Core.Models;
using Newtonsoft.Json;
using Serilog;

namespace Athena.Importer.Provider
{
    public class JsonFilesystemDataProvider<T> : IDataProvider<T> where T : IUniqueObject<Guid>
    {
        private readonly string _dataRoot;

        public JsonFilesystemDataProvider(string dataRoot) =>
            _dataRoot = dataRoot ?? throw new ArgumentNullException(nameof(dataRoot));
        
        public IEnumerable<T> GetData()
        {
            var dataDir = Path.Combine(_dataRoot, typeof(T).Name.ToLowerInvariant());

            if (!Directory.Exists(dataDir))
            {
                Log.Warning("Could not find all or part of the path {dataDir}. Not Importing any {T}", dataDir, typeof(T).Name);
                yield break;
            }

            foreach (var obj in Directory.GetFiles(dataDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                yield return JsonConvert.DeserializeObject<T>(File.ReadAllText(obj));
            }
        }
    }
}