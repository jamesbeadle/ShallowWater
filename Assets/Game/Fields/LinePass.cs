using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class LinePass
    {
        private const int FewestPoints = 2;

        private readonly List<GroundPoint> points;

        public LinePass(List<GroundPoint> points)
        {
            this.points = points;
        }

        public bool HasLength => points.Count >= FewestPoints && First.DistanceTo(Last) > 0;
        public GroundPoint First => points[0];
        public GroundPoint Last => points[points.Count - 1];
        public GroundPoint Middle => points[points.Count / 2];
        public CuttingLine CuttingLine => new CuttingLine(First, Last - First);

        public bool IsStraightWithin(double coverMetres)
        {
            var line = CuttingLine;
            return points.All(point => System.Math.Abs(line.LeftOf(point)) <= coverMetres);
        }

        public static List<LinePass> Through(ConvexOutline outline, FollowedLine line)
        {
            var passes = new List<LinePass>();
            var inside = new List<GroundPoint>();
            foreach (var point in line.PointsWithin(outline.Bounds))
            {
                var isInside = outline.IsAround(point);
                if (isInside) inside.Add(point);
                if (isInside || inside.Count == 0) continue;
                passes.Add(new LinePass(inside));
                inside = new List<GroundPoint>();
            }
            passes.Add(new LinePass(inside));
            return passes.Where(pass => pass.HasLength).ToList();
        }
    }
}
