using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class SparrowModel
    {
        private const string BoatName = "Sparrow";
        private const string ModelName = "Hull, cabin and hold";
        private const string PartsName = "Sparrow";

        public static GameObject Launched(BoatMotion motion, Pound pound)
        {
            var boat = new GameObject(BoatName);
            var helm = boat.AddComponent<BoatController>();
            helm.Launch(motion, pound);
            boat.AddComponent<WakeSignal>();
            boat.AddComponent<WakeTrail>();
            boat.AddComponent<EngineSound>();
            var model = Model(boat.transform);
            model.AddComponent<Riding>().Under(helm);
            var modelPlacement = model.transform;
            Plume.Rising(modelPlacement, TopOf(Stovepipes.ChimneyFoot, Stovepipes.ChimneyTopMetres), SmokeLook.Stove(), SmokeLook.StovePuffs);
            Plume.Rising(modelPlacement, TopOf(Stovepipes.ExhaustFoot, Stovepipes.ExhaustTopMetres), SmokeLook.Exhaust(), SmokeLook.ExhaustPuffs);
            return boat;
        }

        private static GameObject Model(Transform boat)
        {
            var model = new GameObject(ModelName);
            var modelPlacement = model.transform;
            modelPlacement.SetParent(boat, false);
            foreach (var part in ShapeMeshes.BuildEach(PartsName, SparrowShapes.MooredAtTheOrigin()))
            {
                var partPlacement = part.transform;
                partPlacement.SetParent(modelPlacement, false);
            }
            return model;
        }

        private static Vector3 TopOf(WorldPoint foot, double heightMetres)
        {
            return new Vector3((float)foot.East, (float)(foot.Height + heightMetres), (float)foot.North);
        }
    }
}
