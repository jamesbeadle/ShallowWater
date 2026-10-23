namespace ShallowWater.Game.People
{
    public static class Hermite
    {
        private const double Two = 2;
        private const double Three = 3;

        public static double Between(double from, double to, double fromSlope, double toSlope, double along)
        {
            var squared = along * along;
            var cubed = squared * along;
            var fromWeight = Two * cubed - Three * squared + 1;
            var toWeight = -Two * cubed + Three * squared;
            var fromSlopeWeight = cubed - Two * squared + along;
            var toSlopeWeight = cubed - squared;
            return from * fromWeight + to * toWeight + fromSlope * fromSlopeWeight + toSlope * toSlopeWeight;
        }
    }
}
