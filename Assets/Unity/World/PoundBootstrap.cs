using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundBootstrap
    {
        private const double HeadingSouth = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void WakeOnTheBoatAtHopwas()
        {
            var pound = Pound.HuddlesfordToFazeley();
            PoundScenery.Build(pound);
            var mooring = new WaterPosition(PoundLimits.HopwasMooringAlong, 0);
            var motion = new BoatMotion(mooring, HeadingSouth);
            var boat = PoundScenery.Boat();
            boat.AddComponent<BoatController>().Launch(motion, pound);
            Camera().Follow(boat.transform);
        }

        private static FollowCamera Camera()
        {
            var camera = UnityEngine.Camera.main;
            var hasNoCamera = camera == null;
            if (hasNoCamera) camera = HelmCamera();
            return camera.gameObject.AddComponent<FollowCamera>();
        }

        private static UnityEngine.Camera HelmCamera()
        {
            var camera = new GameObject("Helm camera").AddComponent<UnityEngine.Camera>();
            camera.tag = "MainCamera";
            return camera;
        }
    }
}
