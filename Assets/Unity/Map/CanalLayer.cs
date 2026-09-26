using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Unity.Map
{
    public static class CanalLayer
    {
        private const string LayerName = "Canal";

        public static void Dig(GroundLine pound)
        {
            var surfaces = new SurfaceShapes();
            foreach (var band in CanalSection.OfThePound()) surfaces.AddRibbon(pound, band);
            AddCanalsBeyond(surfaces);
            ShapeMeshes.BuildEach(LayerName, surfaces);
        }

        private static void AddCanalsBeyond(SurfaceShapes surfaces)
        {
            if (!OptionalLayer.IsPresent(MapLayers.Canal)) return;
            var beyondThePound = CanalSection.BeyondThePound();
            foreach (var canal in MapLines.Read(MapLayers.Canal)) surfaces.AddRibbon(canal, beyondThePound);
        }
    }
}
