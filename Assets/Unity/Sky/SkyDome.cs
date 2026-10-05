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
        private static readonly int StarShare = Shader.PropertyToID("_StarShare");

        public static void Hang()
        {
            Shader.SetGlobalFloat(CloudCoverProperty, CloudCover);
            RenderSettings.skybox = LookShaders.Made(LookShaders.Sky);
        }

        public static void Paint(SkyColours colours, Vector3 lightHeading, float nightShare)
        {
            Shader.SetGlobalVector(SunDirection, -lightHeading);
            ShaderColours.SetGlobal(SkyZenith, colours.Zenith);
            ShaderColours.SetGlobal(SkyHorizon, colours.Horizon);
            ShaderColours.SetGlobal(SkyHaze, colours.Haze);
            ShaderColours.SetGlobal(SunGlow, colours.Glow);
            ShaderColours.SetGlobal(CloudLight, colours.CloudLight);
            ShaderColours.SetGlobal(CloudShade, colours.CloudShade);
            Shader.SetGlobalFloat(StarShare, nightShare);
        }
    }
}
