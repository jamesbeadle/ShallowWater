using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.Sky
{
    public static class SkyDome
    {
        private const float CloudCover = 0.45f;
        private static readonly int SunDirection = Shader.PropertyToID("_SunDirection");
        private static readonly int SkyZenith = Shader.PropertyToID("_SkyZenith");
        private static readonly int SkyHorizon = Shader.PropertyToID("_SkyHorizon");
        private static readonly int SkyHaze = Shader.PropertyToID("_SkyHaze");
        private static readonly int SunGlow = Shader.PropertyToID("_SunGlow");
        private static readonly int CloudLight = Shader.PropertyToID("_CloudLight");
        private static readonly int CloudShade = Shader.PropertyToID("_CloudShade");
        private static readonly int CloudCoverProperty = Shader.PropertyToID("_CloudCover");

        public static void SpreadOver(Vector3 sunlightHeading)
        {
            Shader.SetGlobalVector(SunDirection, -sunlightHeading);
            ShaderColours.SetGlobal(SkyZenith, SkyPalette.Zenith);
            ShaderColours.SetGlobal(SkyHorizon, SkyPalette.Horizon);
            ShaderColours.SetGlobal(SkyHaze, SkyPalette.Haze);
            ShaderColours.SetGlobal(SunGlow, SkyPalette.SunGlow);
            ShaderColours.SetGlobal(CloudLight, SkyPalette.CloudLight);
            ShaderColours.SetGlobal(CloudShade, SkyPalette.CloudShade);
            Shader.SetGlobalFloat(CloudCoverProperty, CloudCover);
            RenderSettings.skybox = LookShaders.Made(LookShaders.Sky);
            DynamicGI.UpdateEnvironment();
        }
    }
}
