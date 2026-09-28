using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class CrownLook
    {
        private static readonly int AutumnGold = Shader.PropertyToID("_AutumnGold");
        private static readonly int AutumnRust = Shader.PropertyToID("_AutumnRust");
        private static readonly int AutumnRed = Shader.PropertyToID("_AutumnRed");

        public static Material Autumn()
        {
            var material = LookShaders.Made(LookShaders.Crown);
            material.SetColor(LookProperties.Colour, CountryPalette.SummerLeaf);
            material.SetColor(AutumnGold, CountryPalette.AutumnGold);
            material.SetColor(AutumnRust, CountryPalette.AutumnRust);
            material.SetColor(AutumnRed, CountryPalette.AutumnRed);
            material.SetColor(LookProperties.Gaps, CountryPalette.CrownShade);
            material.enableInstancing = true;
            return material;
        }
    }
}
