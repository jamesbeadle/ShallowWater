using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class InnsLayer
    {
        private const string LayerName = "Inns";

        public static void Raise()
        {
            if (!OptionalLayer.IsPresent(MapLayers.Inns)) return;
            var footprints = MapAreas.Read(MapLayers.Inns);
            Solidity.Hardened(ShapeMeshes.BuildEach(LayerName, Inns.Raised(footprints)));
        }
    }
}
