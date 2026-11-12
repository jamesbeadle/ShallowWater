using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Looks
{
    public static class DuskLook
    {
        private static readonly Dictionary<string, int> Tones = new Dictionary<string, int>
        {
            { "--night", Shader.PropertyToID("_Night") },
            { "--umber", Shader.PropertyToID("_Umber") },
            { "--oxblood", Shader.PropertyToID("_Oxblood") },
            { "--brick", Shader.PropertyToID("_Brick") },
            { "--ember", Shader.PropertyToID("_Ember") },
            { "--sunset", Shader.PropertyToID("_Sunset") },
            { "--glow", Shader.PropertyToID("_Glow") }
        };

        public static Material Painted()
        {
            return LookShaders.Made(LookShaders.TitleBackdrop);
        }

        public static void Tint(Material dusk, ICustomStyle brand)
        {
            foreach (var tone in Tones) Tint(dusk, brand, tone.Key, tone.Value);
        }

        private static void Tint(Material dusk, ICustomStyle brand, string token, int property)
        {
            var named = new CustomStyleProperty<Color>(token);
            var isInTheBrand = brand.TryGetValue(named, out var colour);
            if (!isInTheBrand) return;
            Vector4 asPainted = colour;
            dusk.SetVector(property, asPainted);
        }
    }
}
