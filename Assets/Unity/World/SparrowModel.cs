using ShallowWater.Game.Boat;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class SparrowModel
    {
        private const string BoatName = "Sparrow";
        private const string ModelName = "Hull, cabin and hold";
        private const string PartsName = "Sparrow";

        public static GameObject Moored()
        {
            var boat = new GameObject(BoatName);
            var model = new GameObject(ModelName);
            var modelPlacement = model.transform;
            modelPlacement.SetParent(boat.transform, false);
            foreach (var part in ShapeMeshes.BuildEach(PartsName, SparrowShapes.MooredAtTheOrigin()))
            {
                var partPlacement = part.transform;
                partPlacement.SetParent(modelPlacement, false);
            }
            model.AddComponent<Bobbing>();
            return boat;
        }
    }
}
