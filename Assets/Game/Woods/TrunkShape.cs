using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TrunkShape
    {
        private const int Sides = 8;
        private static readonly WorldPoint Foot = new WorldPoint(0, 0, 0);
        private static readonly ProfilePoint[] Profile = { new ProfilePoint(0.5, 0), new ProfilePoint(0.32, 1) };

        public static Shape Tapered()
        {
            return Lathe.Turned(Foot, Profile, Sides);
        }
    }
}
