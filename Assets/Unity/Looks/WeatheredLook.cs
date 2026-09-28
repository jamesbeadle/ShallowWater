using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class WeatheredLook
    {
        private static readonly int PatchMetres = Shader.PropertyToID("_PatchMetres");
        private static readonly int GrainMetres = Shader.PropertyToID("_GrainMetres");
        private static readonly int GrainAmount = Shader.PropertyToID("_GrainAmount");
        private static readonly int AboardSwitch = Shader.PropertyToID("_Aboard");
        private const float MovesWithTheBoat = 1;

        public static Material Of(Tones tones, Weathering weathering)
        {
            var material = LookShaders.Made(LookShaders.Weathered);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(LookProperties.Worn, tones.Worn);
            material.SetColor(LookProperties.Grain, tones.Accent);
            material.SetFloat(PatchMetres, weathering.PatchMetres);
            material.SetFloat(GrainMetres, weathering.GrainMetres);
            material.SetFloat(GrainAmount, weathering.GrainAmount);
            material.SetFloat(LookProperties.Smoothness, weathering.Smoothness);
            material.SetFloat(LookProperties.Metallic, weathering.Metallic);
            material.enableInstancing = true;
            return material;
        }

        public static Material Aboard(Tones tones, Weathering weathering)
        {
            var material = Of(tones, weathering);
            material.SetFloat(AboardSwitch, MovesWithTheBoat);
            return material;
        }
    }
}
