using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class FollowedLine
    {
        private const int PointsPerChunk = 16;

        private readonly List<LineChunk> chunks = new List<LineChunk>();

        public FollowedLine(GroundLine line, double coverMetres)
        {
            CoverMetres = coverMetres;
            var points = line.Points;
            for (var start = 0; start < points.Count - 1; start += PointsPerChunk - 1)
            {
                chunks.Add(new LineChunk(points.Skip(start).Take(PointsPerChunk).ToList()));
            }
        }

        public double CoverMetres { get; }

        public List<GroundPoint> PointsWithin(Bounds bounds)
        {
            var near = chunks.Where(chunk => chunk.IsOverlapping(bounds));
            return near.SelectMany(chunk => chunk.Points).Distinct().ToList();
        }
    }
}
