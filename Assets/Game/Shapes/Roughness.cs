using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Roughness
    {
        private const double CellMetres = 6;
        private const int EastPrime = 73856093;
        private const int NorthPrime = 19349663;
        private const int MixingPrime = 1274126177;
        private const int MixingShift = 13;
        private const int LowBits = 0xFFFF;
        private const double HalfOfTheLowBits = 32767.5;
        private const double Smoothing = 3;
        private const double Twice = 2;

        public static double At(GroundPoint place)
        {
            var east = place.East / CellMetres;
            var north = place.North / CellMetres;
            var westEdge = Math.Floor(east);
            var southEdge = Math.Floor(north);
            var across = Smoothed(east - westEdge);
            var up = Smoothed(north - southEdge);
            var south = Blend(Corner(westEdge, southEdge), Corner(westEdge + 1, southEdge), across);
            var northern = Blend(Corner(westEdge, southEdge + 1), Corner(westEdge + 1, southEdge + 1), across);
            return Blend(south, northern, up);
        }

        private static double Corner(double east, double north)
        {
            unchecked
            {
                var hash = (int)east * EastPrime ^ (int)north * NorthPrime;
                hash = (hash ^ (hash >> MixingShift)) * MixingPrime;
                return (hash & LowBits) / HalfOfTheLowBits - 1;
            }
        }

        private static double Smoothed(double fraction)
        {
            return fraction * fraction * (Smoothing - Twice * fraction);
        }

        private static double Blend(double from, double to, double fraction)
        {
            return from + (to - from) * fraction;
        }
    }
}
