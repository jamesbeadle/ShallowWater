using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class RoofStowage
    {
        private const int RoundSides = 14;
        private const double CanAcross = 0.52;
        private static readonly double[] CansAlong = { -8.25, -7.8 };
        private const double SeatedMetres = 0.02;
        private const double HandleSpanMetres = 0.08;
        private const double HandleRiseMetres = 0.09;
        private const double HandleFootMetres = 0.37;
        private const double HandleRadiusMetres = 0.009;
        private const int HandlePoints = 9;
        private const double SpoutRadiusMetres = 0.018;
        private static readonly Offset SpoutRoot = new Offset(0, 0.27, -0.12);
        private static readonly Offset SpoutTip = new Offset(0, 0.36, -0.2);
        private const double FullTurnRadians = 2 * Math.PI;
        private const double HalfTurnRadians = Math.PI;

        private static readonly ProfilePoint[] BuckbyCan =
        {
            new ProfilePoint(0.12, 0), new ProfilePoint(0.13, 0.02), new ProfilePoint(0.13, 0.26), new ProfilePoint(0.125, 0.28),
            new ProfilePoint(0.095, 0.33), new ProfilePoint(0.075, 0.35), new ProfilePoint(0.075, 0.37), new ProfilePoint(0.095, 0.38),
            new ProfilePoint(0.095, 0.4), new ProfilePoint(0, 0.405)
        };

        public static void Build(SurfaceShapes surfaces)
        {
            foreach (var along in CansAlong) Can(surfaces, new WorldPoint(CanAcross, SparrowForm.RoofHeightAt(CanAcross) - SeatedMetres, along));
            RopeAndMop.Build(surfaces);
        }

        private static void Can(SurfaceShapes surfaces, WorldPoint foot)
        {
            surfaces.Add(Surface.Can, Lathe.Turned(foot, BuckbyCan, RoundSides));
            var handle = Enumerable.Range(0, HandlePoints).Select(point => HandlePoint(foot, HalfTurnRadians * point / (HandlePoints - 1)));
            surfaces.Add(Surface.Can, Tube.Along(handle.ToList(), HandleRadiusMetres, RoundSides / 2));
            var spout = new[] { SpoutRoot.From(foot), SpoutTip.From(foot) };
            surfaces.Add(Surface.Can, Tube.Along(spout, SpoutRadiusMetres, RoundSides / 2));
        }

        private static WorldPoint HandlePoint(WorldPoint foot, double around)
        {
            var across = HandleSpanMetres * Math.Cos(around);
            var up = HandleFootMetres + HandleRiseMetres * Math.Sin(around);
            return new WorldPoint(foot.East + across, foot.Height + up, foot.North);
        }
    }
}
