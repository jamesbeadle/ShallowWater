using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public readonly struct LimbStart
    {
        public LimbStart(WorldPoint foot, Offset heading, double lengthMetres, double radiusMetres)
        {
            Foot = foot;
            Heading = heading;
            LengthMetres = lengthMetres;
            RadiusMetres = radiusMetres;
        }

        public WorldPoint Foot { get; }
        public Offset Heading { get; }
        public double LengthMetres { get; }
        public double RadiusMetres { get; }
    }
}
