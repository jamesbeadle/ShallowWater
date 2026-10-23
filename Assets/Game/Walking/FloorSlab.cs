using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct FloorSlab
    {
        public FloorSlab(GroundRing outline, double heightMetres)
        {
            Outline = outline;
            HeightMetres = heightMetres;
        }

        public GroundRing Outline { get; }
        public double HeightMetres { get; }
        public GroundPoint SouthWest => new GroundPoint(Outline.West, Outline.South);
        public GroundPoint NorthEast => new GroundPoint(Outline.East, Outline.North);

        public bool IsUnder(GroundPoint place)
        {
            return Outline.IsAround(place);
        }
    }
}
