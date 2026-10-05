namespace ShallowWater.Game.Day
{
    public static class Lighting
    {
        private const double HandOverDegrees = -2;
        private const double FullSunDegrees = 10;
        private const double FullMoonDegrees = -12;

        public static SceneLight At(SkyPosition sun)
        {
            var isTheSunLighting = sun.HeightDegrees >= HandOverDegrees;
            if (isTheSunLighting) return Sunlight(sun);
            var moon = sun.Opposite;
            var moonStrength = Shares.Between(sun.HeightDegrees, HandOverDegrees, FullMoonDegrees);
            return new SceneLight(moon, true, moonStrength);
        }

        private static SceneLight Sunlight(SkyPosition sun)
        {
            var sunStrength = Shares.Between(sun.HeightDegrees, HandOverDegrees, FullSunDegrees);
            return new SceneLight(sun, false, sunStrength);
        }
    }
}
