using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class DrizzleLook
    {
        public const int Streaks = 1800;
        public static readonly Vector3 Box = new Vector3(26f, 14f, 26f);
        private const float FallMetresPerSecond = 4.5f;
        private const float StreakLengthMetres = 0.35f;
        private const float StreakWidthMetres = 0.006f;
        private const float Opacity = 0.22f;
        private static readonly Color Rain = new Color(0.62f, 0.63f, 0.64f);
        private static readonly Vector4 WindDrift = new Vector4(0.6f, 0f, 0.25f, 0f);
        private static readonly int BoxProperty = Shader.PropertyToID("_Box");
        private static readonly int Fall = Shader.PropertyToID("_Fall");
        private static readonly int Length = Shader.PropertyToID("_Length");
        private static readonly int Width = Shader.PropertyToID("_Width");
        private static readonly int Drift = Shader.PropertyToID("_Drift");
        private static readonly int OpacityProperty = Shader.PropertyToID("_Opacity");

        public static Material October()
        {
            var drizzle = LookShaders.Made(LookShaders.Drizzle);
            drizzle.SetColor(LookProperties.Colour, Rain);
            drizzle.SetVector(BoxProperty, Box);
            drizzle.SetFloat(Fall, FallMetresPerSecond);
            drizzle.SetFloat(Length, StreakLengthMetres);
            drizzle.SetFloat(Width, StreakWidthMetres);
            drizzle.SetVector(Drift, WindDrift);
            drizzle.SetFloat(OpacityProperty, Opacity);
            return drizzle;
        }
    }
}
