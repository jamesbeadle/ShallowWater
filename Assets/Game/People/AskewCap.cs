using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewCap
    {
        private const double PeakForwardMetres = 0.07;
        private const double PeakHalfWidthMetres = 0.095;
        private const double PeakMetres = 0.205;
        private const double PeakRootForwardMetres = 0.08;
        private const double PeakThicknessMetres = 0.01;
        private const int PeakCorners = 7;
        private static readonly WorldPoint Neck = new WorldPoint(0, 0, 0);
        private static readonly WorldPoint CrownCentre = new WorldPoint(0, 0, -0.012);
        private static readonly Offset CrownShape = new Offset(0.95, 1, 1.12);

        private static readonly ProfilePoint[] Band =
        {
            new ProfilePoint(0.1, 0.19), new ProfilePoint(0.105, 0.215), new ProfilePoint(0.1, 0.235)
        };

        private static readonly ProfilePoint[] Crown =
        {
            new ProfilePoint(0.105, 0.212), new ProfilePoint(0.122, 0.232), new ProfilePoint(0.118, 0.252), new ProfilePoint(0.09, 0.268),
            new ProfilePoint(0.045, 0.276), new ProfilePoint(0, 0.278)
        };

        public static void Wear(SurfaceShapes head)
        {
            head.Add(Surface.Cap, Scaled.Of(Lathe.Turned(Neck, Band, AskewForm.RoundSides), CrownShape));
            head.Add(Surface.Cap, Scaled.Of(Lathe.Turned(CrownCentre, Crown, AskewForm.RoundSides), CrownShape));
            head.AddSolid(Surface.Cap, Peak(), PeakMetres, PeakMetres + PeakThicknessMetres);
        }

        private static GroundRing Peak()
        {
            var corners = Enumerable.Range(0, PeakCorners).Select(corner =>
            {
                var around = Math.PI * corner / (PeakCorners - 1) - Math.PI / 2;
                return new GroundPoint(PeakHalfWidthMetres * Math.Sin(around), PeakRootForwardMetres + PeakForwardMetres * Math.Cos(around));
            });
            return new GroundRing(corners);
        }
    }
}
