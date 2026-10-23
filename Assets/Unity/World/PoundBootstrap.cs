using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Picture;
using ShallowWater.Unity.Player;
using ShallowWater.Unity.Sky;
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
            var scenery = Scenery.Built(pound);
            var land = LandSurvey.Of(pound, scenery);
            var boat = SparrowModel.Launched(MooredAtHopwas(pound), pound);
            var camera = OrbitingCamera();
            boat.AddComponent<ShoreLeave>().Crew(pound, land, camera);
            var screen = OverlayScreen.Raise();
            FarmBootstrap.Begin(boat, scenery, land, camera, screen);
            boat.AddComponent<Hud>().Shows(screen);
            boat.AddComponent<HudClock>().Reads(dayAndNight, screen);
            boat.AddComponent<Radar>().Over(scenery.PeriodMap, camera);
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
