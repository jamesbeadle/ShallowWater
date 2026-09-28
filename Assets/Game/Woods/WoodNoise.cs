using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public static class WoodNoise
    {
        private const long EastPrime = 73856093;
        private const long NorthPrime = 19349663;
        private const long SaltPrime = 83492791;
        private const long MixingPrime = 1274126177;
        private const int FirstShift = 13;
        private const int SecondShift = 16;
        private const long Mask = 0xFFFFFF;

        public static double At(GroundPoint point, double cellMetres, int salt)
        {
            var east = point.East / cellMetres;
            var north = point.North / cellMetres;
            var cellEast = (long)Math.Floor(east);
            var cellNorth = (long)Math.Floor(north);
            var across = Eased(east - cellEast);
            var up = Eased(north - cellNorth);
            var southern = Between(Hash(cellEast, cellNorth, salt), Hash(cellEast + 1, cellNorth, salt), across);
            var northern = Between(Hash(cellEast, cellNorth + 1, salt), Hash(cellEast + 1, cellNorth + 1, salt), across);
            return Between(southern, northern, up);
        }

        private static double Hash(long east, long north, int salt)
        {
            unchecked
            {
                var mixed = east * EastPrime ^ north * NorthPrime ^ salt * SaltPrime;
                mixed = (mixed ^ (mixed >> FirstShift)) * MixingPrime;
                return ((mixed ^ (mixed >> SecondShift)) & Mask) / (double)Mask;
            }
        }

        private static double Eased(double share)
        {
            return share * share * (3 - 2 * share);
        }

        private static double Between(double from, double to, double share)
        {
            return from + (to - from) * share;
        }
    }
}
