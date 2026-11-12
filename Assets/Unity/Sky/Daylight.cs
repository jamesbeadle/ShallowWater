using ShallowWater.Game.Day;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Sky
{
    public static class Daylight
    {
        private const string SkyLightName = "Sun and moon";
        private const float ShadowStrength = 0.5f;
        private const float AmbientIntensity = 1f;
        private const float HazeDensity = 0.0021f;

        public static DayAndNight Rise(TimeOfDay start)
        {
            var light = LightOfTheScene();
            light.type = LightType.Directional;
            light.shadows = LightShadows.Soft;
            light.shadowStrength = ShadowStrength;
            RenderSettings.sun = light;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientIntensity = AmbientIntensity;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = HazeDensity;
            SkyDome.Hang();
            var dayAndNight = light.gameObject.AddComponent<DayAndNight>();
            dayAndNight.Keep(new GameClock(start));
            return dayAndNight;
        }

        private static Light LightOfTheScene()
        {
            var sceneLight = Object.FindFirstObjectByType<Light>();
            var hasALight = sceneLight != null;
            if (hasALight) return sceneLight;
            var skyLight = new GameObject(SkyLightName);
            return skyLight.AddComponent<Light>();
        }
    }
}
