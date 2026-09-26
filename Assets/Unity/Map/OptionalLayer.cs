using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class OptionalLayer
    {
        public static bool IsPresent(string layerName)
        {
            var isPresent = MapFiles.Has(layerName);
            if (isPresent) return true;
            Debug.LogWarning($"There is no {layerName} layer yet, so it is left out: run {MapFiles.RegenerateCommand} from the repository root.");
            return false;
        }
    }
}
