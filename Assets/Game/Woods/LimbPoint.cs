using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public readonly struct LimbPoint
    {
        public LimbPoint(WorldPoint place, Offset heading, double radiusMetres)
        {
            Place = place;
            Heading = heading;
            RadiusMetres = radiusMetres;
        }

        public WorldPoint Place { get; }
        public Offset Heading { get; }
        public double RadiusMetres { get; }
    }
}
