using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TrunkShape
    {
        private const int Sides = 8;
        private const double FootRadius = 0.5;
        private const double TopRadius = 0.32;
        private const double Foot = 0;
        private const double Top = 1;
        private const int PointsPerSide = 2;
        private const double FullTurnRadians = 2 * Math.PI;

        public static Shape Tapered()
        {
            var trunk = new Shape();
            for (var side = 0; side < Sides; side++)
            {
                var around = FullTurnRadians * side / Sides;
                trunk.Add(new WorldPoint(FootRadius * Math.Cos(around), Foot, FootRadius * Math.Sin(around)), new SurfacePlace(around, Foot));
                trunk.Add(new WorldPoint(TopRadius * Math.Cos(around), Top, TopRadius * Math.Sin(around)), new SurfacePlace(around, Top));
            }
            for (var side = 0; side < Sides; side++) Join(trunk, side);
            return trunk;
        }

        private static void Join(Shape trunk, int side)
        {
            var foot = side * PointsPerSide;
            var nextFoot = (side + 1) % Sides * PointsPerSide;
            trunk.AddQuad(foot, nextFoot, foot + 1, nextFoot + 1);
        }
    }
}
