using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class ForeEnd
    {
        private const int RoundSides = 12;
        private const double DeckBoardFootAlong = 7.0;
        private const double DeckBoardSpread = 0.85;
        private const double DeckBoardClearMetres = 0.01;
        private const double DeckBoardApexAboveThePlankMetres = 0.06;
        private const double LampAlong = 7.75;
        private const double LensForwardMetres = 0.104;
        private const double LensHeightMetres = 0.11;
        private const double LensRadiusMetres = 0.065;
        private const int LensSides = 12;
        private const double RimRadiusMetres = 0.007;
        private const double StudAlong = 9.4;
        private const double StudPinHeightMetres = 0.13;
        private const double StudPinHalfLengthMetres = 0.08;
        private const double StudPinRadiusMetres = 0.015;
        private const double FullTurnRadians = 2 * Math.PI;

        private static readonly ProfilePoint[] Lamp =
        {
            new ProfilePoint(0.09, 0), new ProfilePoint(0.1, 0.02), new ProfilePoint(0.1, 0.2), new ProfilePoint(0.115, 0.215),
            new ProfilePoint(0.07, 0.26), new ProfilePoint(0.03, 0.27), new ProfilePoint(0.03, 0.3), new ProfilePoint(0, 0.3)
        };

        private static readonly ProfilePoint[] Stud =
        {
            new ProfilePoint(0.03, 0), new ProfilePoint(0.03, 0.16), new ProfilePoint(0.045, 0.17), new ProfilePoint(0.045, 0.19), new ProfilePoint(0, 0.19)
        };

        public static void Build(SurfaceShapes surfaces)
        {
            DeckBoard(surfaces);
            Headlamp(surfaces);
            TStud(surfaces);
            BowFender.Hang(surfaces);
        }

        private static void DeckBoard(SurfaceShapes surfaces)
        {
            var apexHeight = Sheeting.PlankTopMetres + DeckBoardApexAboveThePlankMetres;
            var apex = new WorldPoint(BoatSides.Amidships, apexHeight, SparrowForm.HoldFrontAlong + DeckBoardClearMetres);
            var reach = Hull.HalfBeamAt(DeckBoardFootAlong) * DeckBoardSpread;
            var foot = Hull.DeckAt(DeckBoardFootAlong) + DeckBoardClearMetres;
            var corners = new[] { apex, new WorldPoint(-reach, foot, DeckBoardFootAlong), new WorldPoint(reach, foot, DeckBoardFootAlong) };
            surfaces.Add(Surface.Cratch, FacingPolygon.Towards(corners, BoatSides.Ahead));
        }

        private static void Headlamp(SurfaceShapes surfaces)
        {
            var foot = new WorldPoint(BoatSides.Amidships, Hull.DeckAt(LampAlong), LampAlong);
            surfaces.Add(Surface.BoatIron, Lathe.Turned(foot, Lamp, RoundSides));
            var centre = new WorldPoint(foot.East, foot.Height + LensHeightMetres, foot.North + LensForwardMetres);
            var lens = Enumerable.Range(0, LensSides).Select(side => LensPoint(centre, LensRadiusMetres, FullTurnRadians * side / LensSides)).ToList();
            surfaces.Add(Surface.LampGlass, FacingPolygon.Towards(lens, BoatSides.Ahead));
            surfaces.Add(Surface.BoatIron, Tube.Around(lens, RimRadiusMetres, RoundSides / 2));
        }

        private static WorldPoint LensPoint(WorldPoint centre, double radius, double around)
        {
            return new WorldPoint(centre.East + radius * Math.Cos(around), centre.Height + radius * Math.Sin(around), centre.North);
        }

        private static void TStud(SurfaceShapes surfaces)
        {
            var foot = new WorldPoint(BoatSides.Amidships, Hull.DeckAt(StudAlong), StudAlong);
            surfaces.Add(Surface.BoatIron, Lathe.Turned(foot, Stud, RoundSides));
            var pinHeight = foot.Height + StudPinHeightMetres;
            var pin = new[] { new WorldPoint(-StudPinHalfLengthMetres, pinHeight, StudAlong), new WorldPoint(StudPinHalfLengthMetres, pinHeight, StudAlong) };
            surfaces.Add(Surface.BoatIron, Tube.Along(pin, StudPinRadiusMetres, RoundSides / 2));
        }
    }
}
