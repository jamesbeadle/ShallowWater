using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Farmyard
{
    public sealed class PostAndRail
    {
        public const double PostHeightMetres = 1.2;
        private const double PostSpacingMetres = 2.2;
        private const double PostRadiusMetres = 0.07;
        private const double RailRadiusMetres = 0.035;
        private const double GateHalfWidthMetres = 1.7;
        private const double FootingHalfWidthMetres = 0.05;
        private const int PostSides = 6;
        private const int RailSides = 6;
        private const double Halfway = 0.5;
        private static readonly double[] RailHeightsMetres = { 0.5, 0.95 };

        private readonly Farmyard yard;
        private readonly List<WorldPoint> postTops = new List<WorldPoint>();
        private readonly List<GroundRing> footings = new List<GroundRing>();

        public PostAndRail(Farmyard yard)
        {
            this.yard = yard;
        }

        public IReadOnlyList<WorldPoint> PostTops => postTops;
        public IReadOnlyList<GroundRing> Footings => footings;

        public void Fence(SurfaceShapes shapes)
        {
            var corners = yard.Corners;
            for (int corner = 0, previous = corners.Count - 1; corner < corners.Count; previous = corner++)
            {
                Run(shapes, corners[previous], corners[corner]);
            }
        }

        private void Run(SurfaceShapes shapes, GroundPoint from, GroundPoint to)
        {
            var length = from.DistanceTo(to);
            var bays = (int)Math.Ceiling(length / PostSpacingMetres);
            var step = (to - from) * (1.0 / bays);
            for (var bay = 0; bay < bays; bay++) Bay(shapes, from + step * bay, from + step * (bay + 1));
        }

        private void Bay(SurfaceShapes shapes, GroundPoint from, GroundPoint to)
        {
            Post(shapes, from);
            var isTheGateway = IsAtTheGate(from) && IsAtTheGate(to);
            if (isTheGateway) return;
            foreach (var height in RailHeightsMetres) Rail(shapes, from, to, height);
            var middle = (from + to) * Halfway;
            footings.Add(Footprints.Oblong(middle, (to - from).Normalised, from.DistanceTo(to) * Halfway, FootingHalfWidthMetres));
        }

        private bool IsAtTheGate(GroundPoint post)
        {
            return post.DistanceTo(yard.Gate) < GateHalfWidthMetres;
        }

        private void Post(SurfaceShapes shapes, GroundPoint foot)
        {
            var profile = new[] { new ProfilePoint(PostRadiusMetres, 0), new ProfilePoint(PostRadiusMetres, PostHeightMetres) };
            shapes.Add(Surface.Timber, Lathe.Turned(yard.Raised(foot, 0), profile, PostSides));
            postTops.Add(yard.Raised(foot, PostHeightMetres));
        }

        private void Rail(SurfaceShapes shapes, GroundPoint from, GroundPoint to, double heightMetres)
        {
            var rail = new[] { yard.Raised(from, heightMetres), yard.Raised(to, heightMetres) };
            shapes.Add(Surface.Timber, Tube.Along(rail, RailRadiusMetres, RailSides));
        }
    }
}
