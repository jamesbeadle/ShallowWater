using System.IO;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class MapFiles
    {
        private const string MapFolder = "map";
        private const string LayerExtension = ".json";
        private const string RegenerateCommand = "python3 -m tools.map.regenerate";

        public static T Layer<T>(string layerName)
        {
            var path = PathTo(layerName + LayerExtension);
            var text = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(text);
        }

        public static byte[] Image(string fileName)
        {
            var path = PathTo(fileName);
            return File.ReadAllBytes(path);
        }

        private static string PathTo(string fileName)
        {
            var path = Path.Combine(Application.streamingAssetsPath, MapFolder, fileName);
            var isMissing = !File.Exists(path);
            if (isMissing) throw new FileNotFoundException($"{path} is missing: run {RegenerateCommand} from the repository root.");
            return path;
        }
    }
}
