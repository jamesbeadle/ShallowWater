using ShallowWater.Game.Paddock;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;

namespace ShallowWater.Unity.World
{
    public static class HopwasPaddock
    {
        private const string LayerName = "Hopwas paddock";

        public static PaddockShapes Raise(Pound pound)
        {
            var paddock = PaddockShapes.Of(Paddock.HopwasOn(pound));
            Gatherings.Of(LayerName, Solidity.Hardened(ShapeMeshes.BuildEach(LayerName, paddock.Shapes)));
            return paddock;
        }
    }
}
