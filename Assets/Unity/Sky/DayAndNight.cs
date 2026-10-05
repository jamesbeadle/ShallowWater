using ShallowWater.Game.Day;
using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.Sky
{
    public sealed class DayAndNight : MonoBehaviour
    {
        private const float SunIntensity = 1.35f;
        private const float MoonIntensity = 0.25f;
        private const float RealSecondsBetweenReflections = 2f;

        private GameClock clock;
        private Light skyLight;
        private float secondsSinceReflections;

        public TimeOfDay Now => clock.Now;

        public void Keep(GameClock gameClock)
        {
            clock = gameClock;
            skyLight = GetComponent<Light>();
            Show();
            DynamicGI.UpdateEnvironment();
        }

        private void Update()
        {
            if (clock == null) return;
            clock.Run(Time.deltaTime);
            Show();
            RefreshReflections();
        }

        private void Show()
        {
            var sun = SunPath.SunAt(clock.Now);
            var stage = DayStages.At(sun);
            var colours = SkyPalette.At(stage);
            Shine(Lighting.At(sun), colours);
            RenderSettings.ambientSkyColor = colours.SkyAbove;
            RenderSettings.ambientEquatorColor = colours.LightAround;
            RenderSettings.ambientGroundColor = colours.GroundBelow;
            RenderSettings.fogColor = colours.Haze;
            var lightPlacement = skyLight.transform;
            SkyDome.Paint(colours, lightPlacement.forward, (float)stage.NightShare);
        }

        private void Shine(SceneLight lighting, SkyColours colours)
        {
            var fullIntensity = lighting.IsMoonlight ? MoonIntensity : SunIntensity;
            skyLight.color = colours.Light;
            skyLight.intensity = fullIntensity * (float)lighting.Strength;
            var lightPlacement = skyLight.transform;
            lightPlacement.rotation = SkyAngles.ShiningFrom(lighting.Position);
        }

        private void RefreshReflections()
        {
            secondsSinceReflections += Time.deltaTime;
            var isDue = secondsSinceReflections >= RealSecondsBetweenReflections;
            if (!isDue) return;
            secondsSinceReflections = 0;
            DynamicGI.UpdateEnvironment();
        }
    }
}
