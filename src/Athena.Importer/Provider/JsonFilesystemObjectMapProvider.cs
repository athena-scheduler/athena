using System;
using System.IO;
using Newtonsoft.Json;

namespace Athena.Importer.Provider
{
    public class JsonFilesystemObjectMapProvider : IObjectMapProvider
    {
        private readonly string _dataRoot;

        public JsonFilesystemObjectMapProvider(string dataRoot) =>
            _dataRoot = dataRoot ?? throw new ArgumentNullException(nameof(dataRoot));
        
        public ObjectMap GetMap()
        {
            var map = Path.Combine(_dataRoot, "object-map.json");

            if (!File.Exists(map))
            {
                throw new ArgumentException($"Could not find all or part of the path {map}", nameof(map));
            }

            return JsonConvert.DeserializeObject<ObjectMap>(File.ReadAllText(map));
        }
    }
}