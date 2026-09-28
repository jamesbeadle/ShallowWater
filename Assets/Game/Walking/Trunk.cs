using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public readonly struct Trunk
    {
        public Trunk(GroundPoint foot, double radiusMetres)
        {
            Foot = foot;
            RadiusMetres = radiusMetres;
        }

        public GroundPoint Foot { get; }
        public double RadiusMetres { get; }

        public bool IsBlocking(GroundPoint place)
        {
            return place.DistanceTo(Foot) < RadiusMetres + WalkingPace.BodyRadiusMetres;
        }
    }
}
