using System;

namespace ShallowWater.Game.Houses
{
    public static class RowSeed
    {
        private const int EastPrime = 73856093;
        private const int NorthPrime = 19349663;

        public static int Of(Plot row)
        {
            var corner = row.FrontCorner;
            unchecked
            {
                return (int)Math.Floor(corner.East) * EastPrime ^ (int)Math.Floor(corner.North) * NorthPrime;
            }
        }
    }
}
