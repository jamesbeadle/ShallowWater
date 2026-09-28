using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Chain
    {
        private const double LinkPitchMetres = 0.034;
        private const double LinkHalfLengthMetres = 0.02;
        private const double LinkHalfWidthMetres = 0.011;
        private const double WireRadiusMetres = 0.0035;
        private const int WireSides = 4;
        private const int PointsPerLink = 8;
        private const double SagMetres = 0.08;
        private const double SagShape = 4;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int EveryOther = 2;

        public static void Hang(SurfaceShapes surfaces, WorldPoint from, WorldPoint to)
        {
            var span = Offset.Between(from, to);
            var links = (int)Math.Ceiling(span.Length / LinkPitchMetres);
            for (var link = 0; link < links; link++)
            {
                var here = SaggingAt(from, span, (link + 0.5) / links);
                var next = SaggingAt(from, span, (link + 1.0) / links);
                Link(surfaces, here, Offset.Between(here, next).Normalised, link % EveryOther == 0);
            }
        }

        private static WorldPoint SaggingAt(WorldPoint from, Offset span, double fraction)
        {
            var droop = Offset.Up * (-SagMetres * SagShape * fraction * (1 - fraction));
            return (span * fraction + droop).From(from);
        }

        private static void Link(SurfaceShapes surfaces, WorldPoint centre, Offset along, bool isLyingFlat)
        {
            var flat = along.Cross(Offset.Up).Normalised;
            var upright = flat.Cross(along).Normalised;
            var across = isLyingFlat ? flat : upright;
            var loop = Enumerable.Range(0, PointsPerLink).Select(point => LoopPoint(centre, along, across, FullTurnRadians * point / PointsPerLink));
            surfaces.Add(Surface.Brass, Tube.Around(loop.ToList(), WireRadiusMetres, WireSides));
        }

        private static WorldPoint LoopPoint(WorldPoint centre, Offset along, Offset across, double around)
        {
            var reach = along * (LinkHalfLengthMetres * Math.Cos(around)) + across * (LinkHalfWidthMetres * Math.Sin(around));
            return reach.From(centre);
        }
    }
}
