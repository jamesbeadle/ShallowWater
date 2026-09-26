using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Roads
{
    public readonly struct ClearedChunk
    {
        private readonly IReadOnlyList<GroundPoint> points;
        private readonly double reachMetres;
        private readonly double west;
        private readonly double east;
        private readonly double south;
        private readonly double north;

        public ClearedChunk(IReadOnlyList<GroundPoint> points, double reachMetres, int owner)
        {
            this.points = points;
            this.reachMetres = reachMetres;
            Owner = owner;
            west = points.Min(point => point.East) - reachMetres;
            east = points.Max(point => point.East) + reachMetres;
            south = points.Min(point => point.North) - reachMetres;
            north = points.Max(point => point.North) + reachMetres;
        }

        public int Owner { get; }

        public bool IsNear(GroundPoint place)
        {
            var isOutsideTheBounds = place.East < west || place.East > east || place.North < south || place.North > north;
            if (isOutsideTheBounds) return false;
            for (var index = 1; index < points.Count; index++)
            {
                var distance = place.DistanceToSegment(points[index - 1], points[index]);
                if (distance <= reachMetres) return true;
            }
            return false;
        }
    }
}
