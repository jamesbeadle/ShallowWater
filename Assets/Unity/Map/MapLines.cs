using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class MapLines
    {
        public static List<GroundLine> Read(string layerName)
        {
            var records = Records(layerName);
            return records.Select(record => record.ToLine()).Where(line => line.IsDrawable).ToList();
        }

        public static IReadOnlyList<GroundLine> ReadIfPresent(string layerName)
        {
            var isPresent = OptionalLayer.IsPresent(layerName);
            return isPresent ? Read(layerName) : new List<GroundLine>();
        }

        public static LineRecord[] Records(string layerName)
        {
            var layer = MapFiles.Layer<LinesRecord>(layerName);
            return layer.lines;
        }
    }
}
