using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Houses
{
    public static class Row
    {
        private const int FrontLeft = 0;
        private const int FrontRight = 1;
        private const int BackRight = 2;

        public static Plot Of(GroundRing footprint)
        {
            var corners = footprint.Corners;
            var frontage = corners[FrontRight] - corners[FrontLeft];
            var depth = corners[FrontRight].DistanceTo(corners[BackRight]);
            return new Plot(corners[FrontLeft], frontage.Normalised, frontage.Length, depth);
        }
    }
}
