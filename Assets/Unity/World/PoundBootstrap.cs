using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.Woods;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundBootstrap
    {
        private const double HeadingTowardsFazeley = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void WakeAtHopwas()
        {
            var centreline = MapLines.Read(MapLayers.Pound).First();
            var pound = Pound.HuddlesfordToFazeley(centreline);
            GroundLayer.Lay();
            LandLinesLayer.Lay(MapLayers.River);
            LandLinesLayer.Lay(MapLayers.Railway);
            RoadsLayer.Lay(pound);
            WoodsLayer.Plant();
            BuildingsLayer.Raise();
            PoundScenery.Sun();
            CanalLayer.Dig(pound);
            HuddlesfordPlanks.Drop(pound);
            GlascoteLock.CloseItsBottomGates(pound);
            FazeleyChain.Hang();
            var motion = new BoatMotion(MooringAtHopwas(pound), HeadingTowardsFazeley);
            var boat = PoundScenery.Boat();
            boat.AddComponent<BoatController>().Launch(motion, pound);
            FollowingCamera().Follow(boat.transform);
        }

        private static WaterPosition MooringAtHopwas(Pound pound)
        {
            var bridge = pound.AlongOf(Landmark.HopwasBridge);
            return new WaterPosition(bridge, 0);
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
