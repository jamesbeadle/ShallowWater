using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class SmokeLook
    {
        public const int StovePuffs = 14;
        public const int ExhaustPuffs = 4;
        private const float StoveLifeSeconds = 9f;
        private const float ExhaustLifeSeconds = ExhaustPuffs;
        private const float StoveRiseMetresPerSecond = 0.4f;
        private const float StoveStartMetres = 0.1f;
        private const float StoveGrowthMetresPerSecond = 0.3f;
        private const float StoveOpacity = 0.3f;
        private const float ExhaustRiseMetresPerSecond = 0.9f;
        private const float ExhaustStartMetres = 0.06f;
        private const float ExhaustGrowthMetresPerSecond = 0.35f;
        private const float ExhaustOpacity = 0.45f;
        private static readonly int Life = Shader.PropertyToID("_Life");
        private static readonly int Puffs = Shader.PropertyToID("_Puffs");
        private static readonly int Rise = Shader.PropertyToID("_Rise");
        private static readonly int StartSize = Shader.PropertyToID("_StartSize");
        private static readonly int Grow = Shader.PropertyToID("_Grow");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        public static Material Stove()
        {
            var smoke = Made(BoatPalette.StoveSmoke, StoveLifeSeconds, StovePuffs);
            return Billowing(smoke, StoveRiseMetresPerSecond, StoveStartMetres, StoveGrowthMetresPerSecond, StoveOpacity);
        }

        public static Material Exhaust()
        {
            var smoke = Made(BoatPalette.ExhaustSmoke, ExhaustLifeSeconds, ExhaustPuffs);
            return Billowing(smoke, ExhaustRiseMetresPerSecond, ExhaustStartMetres, ExhaustGrowthMetresPerSecond, ExhaustOpacity);
        }

        private static Material Made(Color colour, float lifeSeconds, int puffs)
        {
            var smoke = LookShaders.Made(LookShaders.Smoke);
            smoke.SetColor(LookProperties.Colour, colour);
            smoke.SetFloat(Life, lifeSeconds);
            smoke.SetFloat(Puffs, puffs);
            return smoke;
        }

        private static Material Billowing(Material smoke, float riseMetresPerSecond, float startSizeMetres, float growMetresPerSecond, float opacity)
        {
            smoke.SetFloat(Rise, riseMetresPerSecond);
            smoke.SetFloat(StartSize, startSizeMetres);
            smoke.SetFloat(Grow, growMetresPerSecond);
            smoke.SetFloat(Opacity, opacity);
            return smoke;
        }
    }
}
