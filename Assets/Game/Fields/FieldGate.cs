using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class FieldGate
    {
        private const double HangingPostSideMetres = 0.2;
        private const double PostHeightMetres = 1.5;
        private const double PostClearanceMetres = 0.15;
        private const double UprightSideMetres = 0.08;
        private const double UprightHeightMetres = 1.2;
        private const double BraceRadiusMetres = 0.03;
        private const int BraceSides = 4;
        private const double LeafFootingHalfWidthMetres = 0.05;
        private const double Half = 0.5;
        private static readonly double[] RailHeightsMetres = { 0.2, 0.42, 0.64, 0.86, 1.12 };
        private static readonly Measure RailSection = new Measure(0.09, 0.04);

        public static void Add(SurfaceShapes surfaces, GroundPoint hingeSide, GroundPoint latchSide, double swingRadians)
        {
            var along = (latchSide - hingeSide).Normalised;
            Timbers.AddPost(surfaces, hingeSide, along, HangingPostSideMetres, PostHeightMetres);
            Timbers.AddPost(surfaces, latchSide, along, HangingPostSideMetres, PostHeightMetres);
            var leaf = Leaf(hingeSide, latchSide, swingRadians);
            var swung = leaf.Direction(0);
            Timbers.AddPost(surfaces, leaf.Start, swung, UprightSideMetres, UprightHeightMetres);
            Timbers.AddPost(surfaces, leaf.End, swung, UprightSideMetres, UprightHeightMetres);
            foreach (var height in RailHeightsMetres) Timbers.AddRail(surfaces, leaf.Start, leaf.End, RailSection, height);
            surfaces.Add(Surface.Timber, Brace(leaf.Start, leaf.End));
        }

        public static GroundLine Leaf(GroundPoint hingeSide, GroundPoint latchSide, double swingRadians)
        {
            var across = latchSide - hingeSide;
            var swung = GroundPoint.Facing(across.Bearing + swingRadians);
            var heel = hingeSide + swung * PostClearanceMetres;
            var head = hingeSide + swung * (across.Length - PostClearanceMetres);
            return new GroundLine(new[] { heel, head });
        }

        public static IEnumerable<GroundRing> Footings(GroundPoint hingeSide, GroundPoint latchSide, double swingRadians)
        {
            var along = (latchSide - hingeSide).Normalised;
            var leaf = Leaf(hingeSide, latchSide, swingRadians);
            var middle = (leaf.Start + leaf.End) * Half;
            yield return Footprints.Oblong(hingeSide, along, HangingPostSideMetres * Half, HangingPostSideMetres * Half);
            yield return Footprints.Oblong(latchSide, along, HangingPostSideMetres * Half, HangingPostSideMetres * Half);
            yield return Footprints.Oblong(middle, leaf.Direction(0), leaf.Start.DistanceTo(leaf.End) * Half, LeafFootingHalfWidthMetres);
        }

        public static WorldPoint TopRailMiddle(GroundPoint hingeSide, GroundPoint latchSide, double swingRadians)
        {
            var leaf = Leaf(hingeSide, latchSide, swingRadians);
            var middle = (leaf.Start + leaf.End) * Half;
            return new WorldPoint(middle.East, Heights.GroundMetres + RailHeightsMetres.Last(), middle.North);
        }

        private static Shape Brace(GroundPoint heel, GroundPoint head)
        {
            var bottom = new WorldPoint(heel.East, Heights.GroundMetres + RailHeightsMetres.First(), heel.North);
            var top = new WorldPoint(head.East, Heights.GroundMetres + RailHeightsMetres.Last(), head.North);
            return Tube.Along(new[] { bottom, top }, BraceRadiusMetres, BraceSides);
        }
    }
}
