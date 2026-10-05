using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class Dollies
    {
        private const double Along = -10.05;
        private const double InFromTheSideMetres = 0.18;
        private const double TieAboveTheDeckMetres = 0.09;
        private const int RoundSides = 8;
        private static readonly ProfilePoint[] Shape =
        {
            new ProfilePoint(0.035, 0), new ProfilePoint(0.035, 0.12), new ProfilePoint(0.05, 0.13), new ProfilePoint(0.045, 0.15), new ProfilePoint(0, 0.16)
        };
        private static readonly double[] Sides = { BoatSides.Port, BoatSides.Starboard };

        public static HullPoint On(double side)
        {
            return new HullPoint(Along, side * (Hull.HalfBeamAt(Along) - InFromTheSideMetres));
        }

        public static double TieMetres => Hull.DeckAt(Along) + TieAboveTheDeckMetres;

        public static void Build(SurfaceShapes surfaces)
        {
            var deck = Hull.DeckAt(Along);
            foreach (var side in Sides)
            {
                var dolly = On(side);
                var foot = new WorldPoint(dolly.ToStarboard, deck, dolly.Ahead);
                surfaces.Add(Surface.BoatIron, Lathe.Turned(foot, Shape, RoundSides));
            }
        }
    }
}
