using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Boat
{
    public static class BoatSides
    {
        public const double Port = -1;
        public const double Starboard = 1;
        public const double Amidships = 0;
        public static readonly GroundPoint Ahead = new GroundPoint(Amidships, 1);
        public static readonly GroundPoint Astern = new GroundPoint(Amidships, -1);
    }
}
