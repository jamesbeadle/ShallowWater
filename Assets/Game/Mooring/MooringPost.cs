using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public readonly struct MooringPost
    {
        public MooringPost(GroundPoint place, double tieMetres, double radiusMetres, bool isPin)
        {
            Place = place;
            TieMetres = tieMetres;
            RadiusMetres = radiusMetres;
            IsPin = isPin;
        }

        public GroundPoint Place { get; }
        public double TieMetres { get; }
        public double RadiusMetres { get; }
        public bool IsPin { get; }
        public WorldPoint Tie => new WorldPoint(Place.East, TieMetres, Place.North);
    }
}
