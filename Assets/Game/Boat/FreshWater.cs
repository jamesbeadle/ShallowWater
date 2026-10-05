namespace ShallowWater.Game.Boat
{
    public static class FreshWater
    {
        public const double KilogramsPerCubicMetre = 1000;
        private const double Half = 0.5;

        public static double PressureAt(double metresPerSecond)
        {
            return Half * KilogramsPerCubicMetre * metresPerSecond * metresPerSecond;
        }
    }
}
