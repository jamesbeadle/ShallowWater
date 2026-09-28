using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class GradeLook
    {
        private const float Exposure = 0.85f;
        private const float Saturation = 0.86f;
        private const float Vignette = 0.28f;
        private static readonly Vector4 OctoberWarmth = new Vector4(1.04f, 1f, 0.94f, 1f);
        private static readonly int ExposureProperty = Shader.PropertyToID("_Exposure");
        private static readonly int WarmthProperty = Shader.PropertyToID("_Warmth");
        private static readonly int SaturationProperty = Shader.PropertyToID("_Saturation");
        private static readonly int VignetteProperty = Shader.PropertyToID("_Vignette");

        public static Material Autumn()
        {
            var material = LookShaders.Made(LookShaders.Grade);
            material.SetFloat(ExposureProperty, Exposure);
            material.SetVector(WarmthProperty, OctoberWarmth);
            material.SetFloat(SaturationProperty, Saturation);
            material.SetFloat(VignetteProperty, Vignette);
            return material;
        }
    }
}
