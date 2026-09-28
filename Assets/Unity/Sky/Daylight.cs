using ShallowWater.Unity.Looks;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.Sky
{
    public static class Daylight
    {
        private const string SunName = "October sun";
        private const float SunHeightDegrees = 35f;
        private const float SunBearingDegrees = 140f;
        private const float SunIntensity = 1.35f;
        private const float ShadowStrength = 0.82f;
        private const float AmbientIntensity = 1f;
        private const float HazeDensity = 0.0009f;

        public static void Rise()
        {
            var sun = SunOfTheScene();
            Shine(sun);
            LightTheShade();
            Haze();
            var sunPlacement = sun.transform;
            SkyDome.SpreadOver(sunPlacement.forward);
        }

        private static void Shine(Light sun)
        {
            sun.type = LightType.Directional;
            sun.color = SkyPalette.Sunlight;
            sun.intensity = SunIntensity;
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = ShadowStrength;
            var sunPlacement = sun.transform;
            sunPlacement.rotation = Quaternion.Euler(SunHeightDegrees, SunBearingDegrees, 0);
            RenderSettings.sun = sun;
        }

        private static void LightTheShade()
        {
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = SkyPalette.SkyAbove;
            RenderSettings.ambientEquatorColor = SkyPalette.LightAround;
            RenderSettings.ambientGroundColor = SkyPalette.GroundBelow;
            RenderSettings.ambientIntensity = AmbientIntensity;
        }

        private static void Haze()
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = HazeDensity;
            RenderSettings.fogColor = SkyPalette.Haze;
        }

        private static Light SunOfTheScene()
        {
            var sceneLight = Object.FindFirstObjectByType<Light>();
            var hasALight = sceneLight != null;
            if (hasALight) return sceneLight;
            var sun = new GameObject(SunName);
            return sun.AddComponent<Light>();
        }
    }
}
