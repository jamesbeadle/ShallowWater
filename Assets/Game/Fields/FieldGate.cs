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
        private static readonly double[] RailHeightsMetres = { 0.2, 0.42, 0.64, 0.86, 1.12 };
        private static readonly Measure RailSection = new Measure(0.09, 0.04);

        public static void Add(SurfaceShapes surfaces, GroundPoint hingeSide, GroundPoint latchSide, double swingRadians)
        {
            var across = latchSide - hingeSide;
            var along = across.Normalised;
            Timbers.AddPost(surfaces, hingeSide, along, HangingPostSideMetres, PostHeightMetres);
            Timbers.AddPost(surfaces, latchSide, along, HangingPostSideMetres, PostHeightMetres);
            var swung = GroundPoint.Facing(along.Bearing + swingRadians);
            var heel = hingeSide + swung * PostClearanceMetres;
            var head = hingeSide + swung * (across.Length - PostClearanceMetres);
            Timbers.AddPost(surfaces, heel, swung, UprightSideMetres, UprightHeightMetres);
            Timbers.AddPost(surfaces, head, swung, UprightSideMetres, UprightHeightMetres);
            foreach (var height in RailHeightsMetres) Timbers.AddRail(surfaces, heel, head, RailSection, height);
            surfaces.Add(Surface.Timber, Brace(heel, head));
        }

        private static Shape Brace(GroundPoint heel, GroundPoint head)
        {
            var bottom = new WorldPoint(heel.East, Heights.GroundMetres + RailHeightsMetres.First(), heel.North);
            var top = new WorldPoint(head.East, Heights.GroundMetres + RailHeightsMetres.Last(), head.North);
            return Tube.Along(new[] { bottom, top }, BraceRadiusMetres, BraceSides);
        }
    }
}
