using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class StoneLook
    {
        public const float PaviorMetres = 0.35f;
        public const float CopingMetres = 0.9f;
        public const float KerbMetres = 0.9f;
        public const float FlagstoneMetres = 0.75f;
        public const float DressingMetres = 1.4f;
        private static readonly int StoneMetres = Shader.PropertyToID("_StoneMetres");

        public static Material Of(Tones tones, float stoneMetres)
        {
            var material = LookShaders.Made(LookShaders.Stone);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(LookProperties.Variation, tones.Worn);
            material.SetColor(LookProperties.Mortar, tones.Accent);
            material.SetFloat(StoneMetres, stoneMetres);
            return material;
        }
    }
}
