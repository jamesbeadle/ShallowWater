using ShallowWater.Game.Farmyard;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class DunstallFarm
    {
        private const string LayerName = "Dunstall Farm";

        public static FarmyardShapes Raise(Farmyard yard)
        {
            var farmyard = FarmyardShapes.Of(yard);
            var farm = new GameObject(LayerName).transform;
            foreach (var piece in ShapeMeshes.BuildEach(LayerName, farmyard.Shapes))
            {
                var placement = piece.transform;
                placement.SetParent(farm, false);
            }
            return farmyard;
        }
    }
}
