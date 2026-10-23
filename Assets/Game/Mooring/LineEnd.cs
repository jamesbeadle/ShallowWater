using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public readonly struct LineEnd
    {
        public LineEnd(WorldPoint tie, double radiusMetres)
        {
            Tie = tie;
            RadiusMetres = radiusMetres;
        }

        public WorldPoint Tie { get; }
        public double RadiusMetres { get; }
        public GroundPoint Place => Tie.Place;
    }
}
