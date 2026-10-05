using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class MooringPin
    {
        public const double TieAboveTheGroundMetres = 0.14;
        private const double IntoTheGroundMetres = 0.05;
        private const int RoundSides = 8;
        private static readonly ProfilePoint[] Pin =
        {
            new ProfilePoint(0.014, 0), new ProfilePoint(0.014, 0.2), new ProfilePoint(0.03, 0.21), new ProfilePoint(0.028, 0.23), new ProfilePoint(0, 0.235)
        };

        public static SurfaceShapes Knocked(MooringPost post)
        {
            var place = post.Place;
            var ground = post.TieMetres - TieAboveTheGroundMetres;
            var foot = new WorldPoint(place.East, ground - IntoTheGroundMetres, place.North);
            var pin = new SurfaceShapes();
            pin.Add(Surface.BoatIron, Lathe.Turned(foot, Pin, RoundSides));
            return pin;
        }
    }
}
