using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
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
            GroundLayer.Lay();
            LandLinesLayer.Lay(MapLayers.River);
            LandLinesLayer.Lay(MapLayers.Railway);
            LandLinesLayer.Lay(MapLayers.Roads);
            WoodsLayer.Plant();
            BuildingsLayer.Raise();
            PoundScenery.Sun();
            var centreline = MapLines.Read(MapLayers.Pound).First();
            var pound = Pound.HuddlesfordToFazeley(centreline);
            CanalLayer.Dig(centreline);
            var motion = new BoatMotion(MooringAtHopwas(pound), HeadingTowardsFazeley);
            var boat = PoundScenery.Boat();
            boat.AddComponent<BoatController>().Launch(motion, pound);
            FollowingCamera().Follow(boat.transform);
        }

        private static WaterPosition MooringAtHopwas(Pound pound)
        {
            var bridge = pound.WaterPositionAt(GroundPoint.HopwasBridge);
            return new WaterPosition(bridge.Along, 0);
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
