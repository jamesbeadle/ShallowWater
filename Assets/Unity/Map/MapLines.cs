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
            return records.Select(record => record.Line()).Where(line => line.IsDrawable).ToList();
        }

        public static LineRecord[] Records(string layerName)
        {
            var layer = MapFiles.Layer<LinesRecord>(layerName);
            return layer.lines;
        }
    }
}
