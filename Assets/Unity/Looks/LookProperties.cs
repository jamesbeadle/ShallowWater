using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class LookProperties
    {
        public static readonly int Colour = Shader.PropertyToID("_Colour");
        public static readonly int Worn = Shader.PropertyToID("_Worn");
        public static readonly int Grain = Shader.PropertyToID("_Grain");
        public static readonly int Variation = Shader.PropertyToID("_Variation");
        public static readonly int Mortar = Shader.PropertyToID("_Mortar");
        public static readonly int Gaps = Shader.PropertyToID("_Gaps");
        public static readonly int Smoothness = Shader.PropertyToID("_Smoothness");
        public static readonly int Metallic = Shader.PropertyToID("_Metallic");
        public static readonly int Soot = Shader.PropertyToID("_Soot");
        public static readonly int SootColour = Shader.PropertyToID("_SootColour");
    }
}
