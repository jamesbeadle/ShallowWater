using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Looks;

namespace ShallowWater.Unity.Map
{
    public static class VillagesLayer
    {
        private const string LayerName = "Village turf";

        public static void Turf()
        {
            var villages = MapAreas.ReadIfPresent(MapLayers.Villages);
            var turf = new Shape();
            foreach (var village in villages) turf.Append(VillageTurf.Under(village));
            Solidity.Harden(ShapeMeshes.Build(LayerName, turf, Finishes.For(Surface.Grass)));
        }
    }
}
