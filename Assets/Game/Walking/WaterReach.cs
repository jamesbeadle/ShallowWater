using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct WaterReach
    {
        public WaterReach(GroundPoint from, GroundPoint to, double halfWidthMetres)
        {
            From = from;
            To = to;
            HalfWidthMetres = halfWidthMetres;
        }

        public GroundPoint From { get; }
        public GroundPoint To { get; }
        public double HalfWidthMetres { get; }

        public bool IsCovering(GroundPoint place)
        {
            return place.DistanceToSegment(From, To) < HalfWidthMetres + WalkingPace.BodyRadiusMetres;
        }
    }
}
