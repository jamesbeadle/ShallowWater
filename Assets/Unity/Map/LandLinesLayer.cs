using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Unity.Map
{
    public static class LandLinesLayer
    {
        public static void Lay(string layerName)
        {
            var surfaces = new SurfaceShapes();
            foreach (var record in MapLines.Records(layerName)) AddLine(surfaces, record);
            ShapeMeshes.BuildEach(layerName, surfaces);
        }

        private static void AddLine(SurfaceShapes surfaces, LineRecord record)
        {
            var line = record.ToLine();
            if (!line.IsDrawable) return;
            surfaces.AddRibbon(line, LineBands.For(record.kind));
        }
    }
}
