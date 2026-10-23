using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;

namespace ShallowWater.Unity.World
{
    public static class DunstallFarm
    {
        private const string LayerName = "Dunstall Farm";

        public static FarmyardShapes Raise(Pound pound)
        {
            var farmyard = FarmyardShapes.Of(Farmyard.DunstallOn(pound));
            Gatherings.Of(LayerName, Solidity.Hardened(ShapeMeshes.BuildEach(LayerName, farmyard.Shapes)));
            return farmyard;
        }
    }
}
