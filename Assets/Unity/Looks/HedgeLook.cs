using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class HedgeLook
    {
        public const float Haws = 0.08f;
        public const float NoBerries = 0;
        private static readonly int Autumn = Shader.PropertyToID("_Autumn");
        private static readonly int Berries = Shader.PropertyToID("_Berries");
        private static readonly int BerryAmount = Shader.PropertyToID("_BerryAmount");

        public static Material Of(Tones tones, float berryAmount)
        {
            var material = LookShaders.Made(LookShaders.Hedge);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(Autumn, tones.Worn);
            material.SetColor(Berries, tones.Accent);
            material.SetColor(LookProperties.Gaps, CountryPalette.HedgeShade);
            material.SetFloat(BerryAmount, berryAmount);
            return material;
        }
    }
}
