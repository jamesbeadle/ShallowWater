using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class ConvexOutline
    {
        private const double Half = 0.5;
        private const int Third = 3;
        private const double NoMargin = 0;

        private readonly List<GroundPoint> corners;
        private readonly List<Boundary> boundaries;

        public ConvexOutline(IEnumerable<GroundPoint> corners, IEnumerable<Boundary> boundaries)
        {
            this.corners = corners.ToList();
            this.boundaries = boundaries.ToList();
        }

        public IReadOnlyList<GroundPoint> Corners => corners;
        public IReadOnlyList<Boundary> Boundaries => boundaries;
        public int EdgeCount => corners.Count;
        public bool IsDrawable => corners.Count >= Third && Area > 0;
        public double Area => -TwiceSignedArea() * Half;
        public Bounds Bounds => Bounds.Around(corners, NoMargin);

        public static ConvexOutline Rectangle(double west, double east, double south, double north)
        {
            var clockwise = new[] { new GroundPoint(west, south), new GroundPoint(west, north), new GroundPoint(east, north), new GroundPoint(east, south) };
            return new ConvexOutline(clockwise, Enumerable.Repeat(Boundary.Open, clockwise.Length));
        }

        public GroundPoint EdgeStart(int edge)
        {
            return corners[edge];
        }

        public GroundPoint EdgeEnd(int edge)
        {
            return corners[(edge + 1) % corners.Count];
        }

        public GroundPoint EdgeDirection(int edge)
        {
            var run = EdgeEnd(edge) - EdgeStart(edge);
            return run.Normalised;
        }

        public bool IsAround(GroundPoint place)
        {
            for (var edge = 0; edge < corners.Count; edge++)
            {
                var run = EdgeEnd(edge) - EdgeStart(edge);
                var isOnTheLeft = run.Cross(place - EdgeStart(edge)) > NoMargin;
                if (isOnTheLeft) return false;
            }
            return true;
        }

        public GroundPoint Centroid()
        {
            return GroundPoint.MeanOf(corners);
        }

        public double WidthAcross(GroundPoint direction)
        {
            var reaches = corners.Select(corner => corner.Dot(direction)).ToList();
            return reaches.Max() - reaches.Min();
        }

        public GroundPoint LongerOf(GroundPoint grain)
        {
            var across = grain.RightAngleClockwise;
            var isLongerAlongTheGrain = WidthAcross(grain) >= WidthAcross(across);
            return isLongerAlongTheGrain ? grain : across;
        }

        private double TwiceSignedArea()
        {
            var sum = 0.0;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                sum += corners[previous].Cross(corners[index]);
            }
            return sum;
        }
    }
}
