using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class SternGear
    {
        private const double BladeBehindTheSternMetres = 0.5;
        private const double BladeHalfLengthMetres = 0.45;
        private const double BladeHalfThicknessMetres = 0.03;
        private static readonly Rise Blade = new Rise(-0.6, 0.12);
        private const double StockBehindTheSternMetres = 0.08;
        private const int RoundSides = 10;
        private static readonly ProfilePoint[] Stock =
        {
            new ProfilePoint(0.05, 0), new ProfilePoint(0.05, 0.7), new ProfilePoint(0.065, 0.74), new ProfilePoint(0.065, 0.8), new ProfilePoint(0, 0.82)
        };
        private const double StockFootMetres = 0.2;
        private static readonly Offset[] SwanNeck =
        {
            new Offset(0, 0.8, 0), new Offset(0, 0.95, -0.03), new Offset(0, 1.15, 0), new Offset(0, 1.35, 0.08),
            new Offset(0, 1.52, 0.2), new Offset(0, 1.63, 0.36), new Offset(0, 1.69, 0.55), new Offset(0, 1.71, 0.72)
        };
        private static readonly Offset TillerHandleEnd = new Offset(0, 1.74, 1.22);
        private const double SwanNeckRadiusMetres = 0.02;
        private const double HandleRadiusMetres = 0.028;
        private static readonly double[] TurksHeadsMetres = { 0.62, 0.68 };
        private const double TurksHeadRadiusMetres = 0.066;
        private const double TurksHeadRopeMetres = 0.017;
        private const int TurksHeadPoints = 14;
        private static readonly ProfilePoint[] HorseTail =
        {
            new ProfilePoint(0.008, 0), new ProfilePoint(0.035, 0.18), new ProfilePoint(0.05, 0.38), new ProfilePoint(0.03, 0.45), new ProfilePoint(0, 0.46)
        };
        private static readonly Offset HorseTailFoot = new Offset(0, 0.3, -0.07);
        private const double FullTurnRadians = 2 * Math.PI;

        public static void Build(SurfaceShapes surfaces)
        {
            var blade = Footprints.Oblong(Behind(BladeBehindTheSternMetres), BoatSides.Ahead, BladeHalfLengthMetres, BladeHalfThicknessMetres);
            surfaces.AddSolid(Surface.BoatIron, blade, Blade.FootMetres, Blade.TopMetres);
            var stockFoot = new WorldPoint(BoatSides.Amidships, StockFootMetres, Hull.SternAlong - StockBehindTheSternMetres);
            surfaces.Add(Surface.BoatIron, Lathe.Turned(stockFoot, Stock, RoundSides));
            var neck = SwanNeck.Select(bend => bend.From(stockFoot)).ToList();
            surfaces.Add(Surface.Brass, Tube.Along(neck, SwanNeckRadiusMetres, RoundSides / 2));
            var handle = new[] { neck.Last(), TillerHandleEnd.From(stockFoot) };
            surfaces.Add(Surface.Deck, Tube.Along(handle, HandleRadiusMetres, RoundSides / 2));
            foreach (var height in TurksHeadsMetres) TurksHead(surfaces, stockFoot, height);
            surfaces.Add(Surface.HorseTail, Lathe.Turned(HorseTailFoot.From(stockFoot), HorseTail, RoundSides));
        }

        private static GroundPoint Behind(double metres)
        {
            return new GroundPoint(BoatSides.Amidships, Hull.SternAlong - metres);
        }

        private static void TurksHead(SurfaceShapes surfaces, WorldPoint stockFoot, double height)
        {
            var loop = Enumerable.Range(0, TurksHeadPoints).Select(point =>
            {
                var around = FullTurnRadians * point / TurksHeadPoints;
                var reach = new Offset(TurksHeadRadiusMetres * Math.Cos(around), height, TurksHeadRadiusMetres * Math.Sin(around));
                return reach.From(stockFoot);
            });
            surfaces.Add(Surface.Rope, Tube.Around(loop.ToList(), TurksHeadRopeMetres, RoundSides / 2));
        }
    }
}
