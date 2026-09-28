using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class RopeAndMop
    {
        private const double CoilAcross = 0.35;
        private const double CoilAlong = -4.75;
        private const int CoilTurns = 4;
        private const double CoilRadiusMetres = 0.16;
        private const double CoilTighteningMetres = 0.012;
        private const double RopeRadiusMetres = 0.017;
        private const int RopeSides = 6;
        private const int PointsPerTurn = 18;
        private const double MopAcross = -0.62;
        private const double MopHandleFromAlong = -6.5;
        private const double MopHandleToAlong = -5.1;
        private const double MopHandleRadiusMetres = 0.018;
        private const double MopHeadLengthMetres = 0.28;
        private const double MopHeadRadiusMetres = 0.07;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int RopeDiametersPerTurn = 2;

        public static void Build(SurfaceShapes surfaces)
        {
            var roof = SparrowForm.RoofHeightAt(CoilAcross);
            for (var turn = 0; turn < CoilTurns; turn++)
            {
                var height = roof + RopeRadiusMetres * (1 + RopeDiametersPerTurn * turn);
                Turn(surfaces, height, CoilRadiusMetres - CoilTighteningMetres * turn);
            }
            Mop(surfaces);
        }

        private static void Turn(SurfaceShapes surfaces, double height, double radius)
        {
            var loop = Enumerable.Range(0, PointsPerTurn).Select(point =>
            {
                var around = FullTurnRadians * point / PointsPerTurn;
                return new WorldPoint(CoilAcross + radius * Math.Cos(around), height, CoilAlong + radius * Math.Sin(around));
            });
            surfaces.Add(Surface.TarredRope, Tube.Around(loop.ToList(), RopeRadiusMetres, RopeSides));
        }

        private static void Mop(SurfaceShapes surfaces)
        {
            var roof = SparrowForm.RoofHeightAt(MopAcross);
            var handleHeight = roof + MopHandleRadiusMetres;
            var handle = new[] { new WorldPoint(MopAcross, handleHeight, MopHandleFromAlong), new WorldPoint(MopAcross, handleHeight, MopHandleToAlong) };
            surfaces.Add(Surface.Deck, Tube.Along(handle, MopHandleRadiusMetres, RopeSides));
            var headHeight = roof + MopHeadRadiusMetres;
            var headEnd = MopHandleFromAlong - MopHeadLengthMetres;
            var head = new[] { new WorldPoint(MopAcross, headHeight, MopHandleFromAlong), new WorldPoint(MopAcross, headHeight, headEnd) };
            surfaces.Add(Surface.Rope, Tube.Along(head, MopHeadRadiusMetres, RopeSides * 2));
        }
    }
}
