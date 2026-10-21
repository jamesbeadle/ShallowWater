using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class CentreStud
    {
        public const double Along = 0;
        public const double TieMetres = 0.13;
        private const double PinHalfLengthMetres = 0.08;
        private const double PinRadiusMetres = 0.015;
        private const int RoundSides = 12;
        private static readonly ProfilePoint[] Stud =
        {
            new ProfilePoint(0.03, 0), new ProfilePoint(0.03, 0.16), new ProfilePoint(0.045, 0.17), new ProfilePoint(0.045, 0.19), new ProfilePoint(0, 0.19)
        };

        public static readonly HullPoint OnTheHull = new HullPoint(Along, BoatSides.Amidships);
        public static readonly double TieHeightMetres = Sheeting.PlankTopMetres + TieMetres;
        public static readonly WorldPoint Foot = new WorldPoint(BoatSides.Amidships, Sheeting.PlankTopMetres, Along);
        public static readonly WorldPoint Tie = new WorldPoint(BoatSides.Amidships, TieHeightMetres, Along);

        public static void Build(SurfaceShapes surfaces)
        {
            surfaces.Add(Surface.BoatIron, Lathe.Turned(Foot, Stud, RoundSides));
            var pin = new[] { new WorldPoint(-PinHalfLengthMetres, Tie.Height, Along), new WorldPoint(PinHalfLengthMetres, Tie.Height, Along) };
            surfaces.Add(Surface.BoatIron, Tube.Along(pin, PinRadiusMetres, RoundSides / 2));
        }
    }
}
