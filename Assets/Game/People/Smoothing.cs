namespace ShallowWater.Game.People
{
    public static class Smoothing
    {
        private const double Rise = 3;
        private const double Fall = 2;

        public static double Step(double along)
        {
            return along * along * (Rise - Fall * along);
        }
    }
}
