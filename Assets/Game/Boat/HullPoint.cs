using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Boat
{
    public readonly struct HullPoint
    {
        public HullPoint(double ahead, double toStarboard)
        {
            Ahead = ahead;
            ToStarboard = toStarboard;
        }

        public double Ahead { get; }
        public double ToStarboard { get; }

        public static HullPoint On(BoatMotion boat, GroundPoint place)
        {
            var away = place - boat.Position;
            return new HullPoint(away.Dot(boat.Heading), away.Dot(boat.Starboard));
        }
    }
}
