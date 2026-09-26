using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class BuildingsLayer
    {
        private const string LayerName = "Buildings";

        public static void Raise()
        {
            var footprints = MapAreas.Read(MapLayers.Buildings);
            ShapeMeshes.BuildEach(LayerName, Buildings.Raised(footprints));
        }
    }
}
