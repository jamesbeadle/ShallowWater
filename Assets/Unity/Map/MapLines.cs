using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class MapLines
    {
        public static List<GroundLine> Read(string layerName)
        {
            var record = MapFiles.Layer<LinesRecord>(layerName);
            var lines = record.lines.Select(line => line.Line());
            return lines.Where(line => line.IsDrawable).ToList();
        }
    }
}
