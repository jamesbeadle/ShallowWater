using System.Linq;
using ShallowWater.Game.Saving;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Picture;
using ShallowWater.Unity.Player;
using ShallowWater.Unity.Saving;
using ShallowWater.Unity.Sky;
using UnityEngine;
using UnityEngine.UIElements;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Unity.World
{
    public static class PoundBootstrap
    {
        public static void Wake(GameStart start, VisualElement screen)
        {
            var centreline = MapLines.Read(MapLayers.Pound).First();
            var pound = CanalPound.HuddlesfordToFazeley(centreline);
            var dayAndNight = Daylight.Rise(start.Time);
            var scenery = Scenery.Built(pound);
            var land = LandSurvey.Of(pound, scenery);
            var boat = SparrowModel.Launched(start.BoatOn(pound), pound);
            var camera = OrbitingCamera();
            boat.AddComponent<ShoreLeave>().Crew(pound, land, camera);
            boat.AddComponent<PocketBook>().Opens(screen, dayAndNight, pound);
            FarmBootstrap.Begin(boat, scenery, land, camera, screen);
            boat.AddComponent<Hud>().Shows(screen);
            boat.AddComponent<HudClock>().Reads(dayAndNight, screen);
            boat.AddComponent<Radar>().Over(scenery.PeriodMap, camera);
            boat.AddComponent<ControlsKey>().Shows(screen);
            KeepTheLog(boat, pound, dayAndNight, screen);
        }

        private static void KeepTheLog(GameObject boat, CanalPound pound, DayAndNight dayAndNight, VisualElement screen)
        {
            var saver = boat.AddComponent<GameSaver>();
            saver.Keeps(pound, dayAndNight);
            var notice = new SavedNotice(screen);
            saver.Saved += notice.Show;
        }

        private static OrbitCamera OrbitingCamera()
        {
            var camera = HelmCamera.Main();
            HelmPicture.Frame(camera);
            Drizzle.Falling(camera);
            var listening = camera.GetComponent<AudioListener>();
            if (listening == null) camera.gameObject.AddComponent<AudioListener>();
            return camera.gameObject.AddComponent<OrbitCamera>();
        }
    }
}
