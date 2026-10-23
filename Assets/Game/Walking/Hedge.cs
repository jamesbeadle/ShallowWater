using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct Hedge
    {
        public Hedge(GroundLine line, double halfWidthMetres)
        {
            Line = line;
            HalfWidthMetres = halfWidthMetres;
            Reach = LineReach.Around(line, halfWidthMetres + WalkingPace.BodyRadiusMetres);
        }

        public GroundLine Line { get; }
        public double HalfWidthMetres { get; }
        public LineReach Reach { get; }

        public bool IsBlocking(GroundPoint place)
        {
            return Line.DistanceTo(place) < HalfWidthMetres + WalkingPace.BodyRadiusMetres;
        }
    }
}
