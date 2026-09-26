using System.IO;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class MapFiles
    {
        public const string RegenerateCommand = "python3 -m tools.map";
        private const string MapFolder = "map";
        private const string LayerExtension = ".json";

        public static bool Has(string layerName)
        {
            var path = PathOf(layerName + LayerExtension);
            return File.Exists(path);
        }

        public static T Layer<T>(string layerName)
        {
            var path = ExistingPathOf(layerName + LayerExtension);
            var text = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(text);
        }

        public static byte[] Image(string fileName)
        {
            var path = ExistingPathOf(fileName);
            return File.ReadAllBytes(path);
        }

        private static string PathOf(string fileName)
        {
            return Path.Combine(Application.streamingAssetsPath, MapFolder, fileName);
        }

        private static string ExistingPathOf(string fileName)
        {
            var path = PathOf(fileName);
            var isMissing = !File.Exists(path);
            if (isMissing) throw new FileNotFoundException($"{path} is missing: run {RegenerateCommand} from the repository root.");
            return path;
        }
    }
}
