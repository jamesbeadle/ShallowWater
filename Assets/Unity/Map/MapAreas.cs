using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class MapAreas
    {
        public static IReadOnlyList<Area> ReadIfPresent(string layerName)
        {
            var isPresent = OptionalLayer.IsPresent(layerName);
            return isPresent ? Read(layerName) : new List<Area>();
        }

        public static List<Area> Read(string layerName)
        {
            var layer = MapFiles.Layer<AreasRecord>(layerName);
            return layer.areas.Select(area => area.ToArea()).Where(area => area.IsDrawable).ToList();
        }
    }
}
