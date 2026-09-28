using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class BarkLook
    {
        private static readonly int Fissure = Shader.PropertyToID("_Fissure");
        private static readonly int Lichen = Shader.PropertyToID("_Lichen");
        private static readonly int Moss = Shader.PropertyToID("_Moss");
        private static readonly int Upper = Shader.PropertyToID("_Upper");
        private static readonly int Pattern = Shader.PropertyToID("_Pattern");
        private static readonly int FurrowsAround = Shader.PropertyToID("_FurrowsAround");
        private static readonly int FurrowMetres = Shader.PropertyToID("_FurrowMetres");
        private static readonly int Relief = Shader.PropertyToID("_Relief");
        private static readonly int UpperFromMetres = Shader.PropertyToID("_UpperFromMetres");
        private static readonly int FootMetres = Shader.PropertyToID("_FootMetres");
        private static readonly int Lichens = Shader.PropertyToID("_Lichens");

        public static Material Oak()
        {
            return Made(WoodsPalette.OakBark, BarkPatterns.OakFurrows);
        }

        public static Material Ash()
        {
            return Made(WoodsPalette.AshBark, BarkPatterns.AshNetting);
        }

        public static Material Birch()
        {
            var material = Made(WoodsPalette.BirchBark, BarkPatterns.BirchPaper);
            material.SetFloat(FootMetres, BarkPatterns.BirchFootMetres);
            return material;
        }

        public static Material Pine()
        {
            var material = Made(WoodsPalette.PineBark, BarkPatterns.PinePlates);
            material.SetColor(Upper, WoodsPalette.PineUpperBark);
            material.SetFloat(UpperFromMetres, BarkPatterns.PineUpperFromMetres);
            return material;
        }

        public static Material Hazel()
        {
            return Made(WoodsPalette.HazelBark, BarkPatterns.HazelSmooth);
        }

        private static Material Made(Tones tones, BarkPattern pattern)
        {
            var material = LookShaders.Made(LookShaders.Bark);
            material.SetColor(LookProperties.Colour, tones.Main);
            material.SetColor(Fissure, tones.Worn);
            material.SetColor(Lichen, tones.Accent);
            material.SetColor(Moss, WoodsPalette.Moss);
            material.SetFloat(Pattern, pattern.Kind);
            material.SetFloat(FurrowsAround, pattern.FurrowsAround);
            material.SetFloat(FurrowMetres, pattern.FurrowMetres);
            material.SetFloat(Relief, pattern.ReliefMetres);
            material.SetFloat(Lichens, pattern.Lichens);
            material.enableInstancing = true;
            return material;
        }
    }
}
