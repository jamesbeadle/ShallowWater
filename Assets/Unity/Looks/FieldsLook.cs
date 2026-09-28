using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class FieldsLook
    {
        private static readonly int Pasture = Shader.PropertyToID("_Pasture");
        private static readonly int PastureWorn = Shader.PropertyToID("_PastureWorn");
        private static readonly int Stubble = Shader.PropertyToID("_Stubble");
        private static readonly int Plough = Shader.PropertyToID("_Plough");
        private static readonly int Hedgerow = Shader.PropertyToID("_Hedgerow");

        public static Material Countryside()
        {
            var material = LookShaders.Made(LookShaders.Fields);
            material.SetColor(Pasture, CountryPalette.Pasture);
            material.SetColor(PastureWorn, CountryPalette.WornPasture);
            material.SetColor(Stubble, CountryPalette.Stubble);
            material.SetColor(Plough, CountryPalette.Ploughland);
            material.SetColor(Hedgerow, CountryPalette.Hedgerow);
            return material;
        }
    }
}
