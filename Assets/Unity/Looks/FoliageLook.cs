using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class FoliageLook
    {
        private static readonly int Turning = Shader.PropertyToID("_Turning");
        private static readonly int Turned = Shader.PropertyToID("_Turned");
        private static readonly int Autumn = Shader.PropertyToID("_Autumn");
        private static readonly int LeafShape = Shader.PropertyToID("_LeafShape");
        private static readonly int LeafMetres = Shader.PropertyToID("_LeafMetres");
        private const float OakLeaf = 0;
        private const float AshLeaflet = 1;
        private const float BirchLeaf = 2;
        private const float Needles = 3;
        private const float HazelLeaf = 4;
        private const float OakLeafMetres = 0.22f;
        private const float AshLeafletMetres = 0.2f;
        private const float BirchLeafMetres = 0.14f;
        private const float TuftMetres = 0.32f;
        private const float HazelLeafMetres = 0.24f;
        private const float OakAutumn = 0.42f;
        private const float AshAutumn = 0.3f;
        private const float BirchAutumn = 0.7f;
        private const float PineAutumn = 0.1f;
        private const float HazelAutumn = 0.55f;

        public static Material Oak()
        {
            return Made(WoodsPalette.OakLeaves, OakLeaf, OakLeafMetres, OakAutumn);
        }

        public static Material Ash()
        {
            return Made(WoodsPalette.AshLeaves, AshLeaflet, AshLeafletMetres, AshAutumn);
        }

        public static Material Birch()
        {
            return Made(WoodsPalette.BirchLeaves, BirchLeaf, BirchLeafMetres, BirchAutumn);
        }

        public static Material Pine()
        {
            return Made(WoodsPalette.PineNeedles, Needles, TuftMetres, PineAutumn);
        }

        public static Material Hazel()
        {
            return Made(WoodsPalette.HazelLeaves, HazelLeaf, HazelLeafMetres, HazelAutumn);
        }

        private static Material Made(Tones tones, float leafShape, float leafMetres, float autumn)
        {
            var material = LookShaders.Made(LookShaders.Foliage);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(Turning, tones.Worn);
            material.SetColor(Turned, tones.Accent);
            material.SetFloat(LeafShape, leafShape);
            material.SetFloat(LeafMetres, leafMetres);
            material.SetFloat(Autumn, autumn);
            material.enableInstancing = true;
            return material;
        }
    }
}
