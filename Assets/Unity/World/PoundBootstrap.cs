using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundBootstrap
    {
        private const double HeadingTowardsFazeley = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void WakeAtHopwas()
        {
            GroundLayer.Lay();
            var pound = Pound.HuddlesfordToFazeley();
            PoundScenery.Build(pound);
            var mooring = new WaterPosition(PoundLimits.HopwasMooringAlong, 0);
            var motion = new BoatMotion(mooring, HeadingTowardsFazeley);
            var boat = PoundScenery.Boat();
            boat.AddComponent<BoatController>().Launch(motion, pound);
            FollowingCamera().Follow(boat.transform);
        }

        private static FollowCamera FollowingCamera()
        {
            var camera = Camera.main;
            var hasNoCamera = camera == null;
            if (hasNoCamera) camera = HelmCamera();
            return camera.gameObject.AddComponent<FollowCamera>();
        }

        private static Camera HelmCamera()
        {
            var camera = new GameObject("Helm camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            return camera;
        }
    }
}
