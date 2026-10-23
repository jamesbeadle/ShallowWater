using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Coil
    {
        private const double FullTurnRadians = 2 * Math.PI;
        private const int PointsPerTurn = 16;
        private const double Flat = 0;

        public static List<WorldPoint> Rising(WorldPoint axis, double radiusMetres, double fromBearing, double turns, double risePerTurnMetres)
        {
            return Turned(axis, fromBearing, turns, turn => radiusMetres, turn => risePerTurnMetres * turn);
        }

        public static List<WorldPoint> Flaked(WorldPoint axis, double outerRadiusMetres, double fromBearing, double turns, double inwardsPerTurnMetres)
        {
            return Turned(axis, fromBearing, turns, turn => outerRadiusMetres - inwardsPerTurnMetres * turn, turn => Flat);
        }

        private static List<WorldPoint> Turned(WorldPoint axis, double fromBearing, double turns, Func<double, double> radiusAt, Func<double, double> riseAt)
        {
            var points = (int)Math.Ceiling(turns * PointsPerTurn);
            var coil = new List<WorldPoint>(points + 1);
            for (var point = 0; point <= points; point++)
            {
                var turn = turns * point / points;
                var around = GroundPoint.Facing(fromBearing + turn * FullTurnRadians) * radiusAt(turn);
                coil.Add(new WorldPoint(axis.East + around.East, axis.Height + riseAt(turn), axis.North + around.North));
            }
            return coil;
        }
    }
}
