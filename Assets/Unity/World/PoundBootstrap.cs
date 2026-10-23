using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Picture;
using ShallowWater.Unity.Player;
using ShallowWater.Unity.Sky;
using ShallowWater.Unity.Woods;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundBootstrap
    {
        private const string MainCameraTag = "MainCamera";
        private const double MooredOffTheBankMetres = 0.3;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void WakeAtHopwas()
        {
            var centreline = MapLines.Read(MapLayers.Pound).First();
            var pound = Pound.HuddlesfordToFazeley(centreline);
            var dayAndNight = Daylight.Rise();
            var countryside = FieldsLayer.Lay(pound);
            var periodMap = GroundLayer.Lay(countryside.Fields);
            LandLinesLayer.Lay(MapLayers.River);
            LandLinesLayer.Lay(MapLayers.Railway);
            RoadsLayer.Lay(pound);
            var trees = WoodsLayer.Plant(countryside.HedgerowTrees);
            BuildingsLayer.Raise();
            CanalLayer.Dig(pound);
            HuddlesfordPlanks.Drop(pound);
            GlascoteLock.CloseItsBottomGates(pound);
            FazeleyChain.Hang();
            var yard = Farmyard.DunstallOn(pound);
            var farm = DunstallFarm.Raise(yard);
            var land = LandSurvey.Of(pound, trees, farm.Footings);
            var boat = SparrowModel.Launched(MooredAtHopwas(pound), pound);
            var camera = OrbitingCamera();
            boat.AddComponent<ShoreLeave>().Crew(pound, land, camera);
            var screen = OverlayScreen.Raise();
            boat.AddComponent<PocketBook>().Opens(screen, dayAndNight, pound);
            FarmBootstrap.Begin(boat, yard, farm, land, camera, screen);
            boat.AddComponent<Hud>().Shows(screen);
            boat.AddComponent<HudClock>().Reads(dayAndNight, screen);
            boat.AddComponent<Radar>().Over(periodMap, camera);
            boat.AddComponent<ControlsKey>().Shows(screen);
        }

        private static BoatMotion MooredAtHopwas(Pound pound)
        {
            var bridge = pound.AlongOf(Landmark.HopwasBridge);
            var againstTheTowpath = PoundLimits.TowpathSide * (pound.HalfWidth - BoatSize.BeamMetres / 2 - MooredOffTheBankMetres);
            var mooring = pound.GroundPointAt(new WaterPosition(bridge, againstTheTowpath));
            return new BoatMotion(mooring, pound.BearingAt(bridge));
        }

        private static OrbitCamera OrbitingCamera()
        {
            var camera = Camera.main;
            var hasNoCamera = camera == null;
            if (hasNoCamera) camera = HelmCamera();
            HelmPicture.Frame(camera);
            var listening = camera.GetComponent<AudioListener>();
            if (listening == null) camera.gameObject.AddComponent<AudioListener>();
            return camera.gameObject.AddComponent<OrbitCamera>();
        }

        private static Camera HelmCamera()
        {
            var camera = new GameObject("Helm camera").AddComponent<Camera>();
            camera.tag = MainCameraTag;
            return camera;
        }
    }
}
