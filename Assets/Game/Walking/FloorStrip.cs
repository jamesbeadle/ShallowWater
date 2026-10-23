using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct FloorStrip
    {
        public FloorStrip(GroundLine line, double halfWidthMetres, double heightMetres)
        {
            Line = line;
            HalfWidthMetres = halfWidthMetres;
            HeightMetres = heightMetres;
            Reach = LineReach.Around(line, halfWidthMetres);
        }

        public GroundLine Line { get; }
        public double HalfWidthMetres { get; }
        public double HeightMetres { get; }
        public LineReach Reach { get; }

        public bool IsUnder(GroundPoint place)
        {
            return Line.DistanceTo(place) < HalfWidthMetres;
        }
    }
}
