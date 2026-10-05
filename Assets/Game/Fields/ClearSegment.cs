using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct ClearSegment
    {
        private readonly GroundPoint from;
        private readonly GroundPoint to;
        private readonly double reachMetres;

        public ClearSegment(GroundPoint from, GroundPoint to, double reachMetres)
        {
            this.from = from;
            this.to = to;
            this.reachMetres = reachMetres;
        }

        public GroundPoint SouthWest => new GroundPoint(Math.Min(from.East, to.East) - reachMetres, Math.Min(from.North, to.North) - reachMetres);
        public GroundPoint NorthEast => new GroundPoint(Math.Max(from.East, to.East) + reachMetres, Math.Max(from.North, to.North) + reachMetres);

        public bool IsNear(GroundPoint place)
        {
            return place.DistanceToSegment(from, to) <= reachMetres;
        }
    }
}
