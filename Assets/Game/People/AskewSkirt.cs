using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewSkirt
    {
        private const double FaceRight = 0;
        private const double FaceForward = Math.PI / 2;
        private const double FaceLeft = Math.PI;
        private const double FaceBack = 3 * Math.PI / 2;
        private const double UnderTheOverlapMetres = 0.006;
        private const double PocketFlapHalfSpreadRadians = 0.28;
        private const double PocketFlapProudMetres = 0.012;
        private const double PocketFlapTopMetres = 0.07;
        private const double PocketFlapBottomMetres = 0.13;
        private const double ButtonRadiusMetres = 0.012;
        private const double ButtonRowHalfWidthMetres = 0.055;
        private const double ButtonBelowTheWaistMetres = 0.06;
        private const double ButtonProudMetres = 0.004;
        private const double AtTheWaist = 0;
        private static readonly WorldPoint Waist = new WorldPoint(0, 0, 0);
        private static readonly Offset BodyShape = new Offset(1, 1, AskewForm.BodyDepthShare);

        private static readonly ProfilePoint[] Hang =
        {
            new ProfilePoint(0.27, -0.6), new ProfilePoint(0.255, -0.5), new ProfilePoint(0.235, -0.36), new ProfilePoint(0.215, -0.2), new ProfilePoint(0.2, -0.08), new ProfilePoint(0.19, 0)
        };

        public static SurfaceShapes Hung(FigureSide side, Outfit outfit)
        {
            var isTheOverlappingSide = side == FigureSide.Left;
            var tuck = isTheOverlappingSide ? 0 : UnderTheOverlapMetres;
            var from = isTheOverlappingSide ? FaceForward - outfit.FrontOverlapRadians : FaceBack;
            var to = isTheOverlappingSide ? FaceBack : FaceForward + 2 * Math.PI;
            var skirt = new SurfaceShapes();
            var panel = Between(outfit.SkirtMetres, AtTheWaist, tuck);
            skirt.Add(outfit.Coat, Scaled.Of(Lathe.TurnedThrough(Waist, panel, AskewForm.RoundSides, from, to), BodyShape));
            var flapCentre = isTheOverlappingSide ? FaceLeft : FaceRight;
            var flap = Between(PocketFlapBottomMetres, PocketFlapTopMetres, -PocketFlapProudMetres);
            skirt.Add(outfit.Coat, Scaled.Of(Lathe.TurnedThrough(Waist, flap, AskewForm.RoundSides, flapCentre - PocketFlapHalfSpreadRadians, flapCentre + PocketFlapHalfSpreadRadians), BodyShape));
            if (isTheOverlappingSide && outfit.IsDoubleBreasted) Button(skirt);
            return skirt;
        }

        private static ProfilePoint[] Between(double bottomBelowTheWaist, double topBelowTheWaist, double insetMetres)
        {
            var cut = new List<ProfilePoint> { new ProfilePoint(Profiles.RadiusAt(Hang, -bottomBelowTheWaist) - insetMetres, -bottomBelowTheWaist) };
            var within = Hang.Where(point => -point.HeightMetres < bottomBelowTheWaist && -point.HeightMetres > topBelowTheWaist);
            cut.AddRange(within.Select(point => new ProfilePoint(point.RadiusMetres - insetMetres, point.HeightMetres)));
            cut.Add(new ProfilePoint(Profiles.RadiusAt(Hang, -topBelowTheWaist) - insetMetres, -topBelowTheWaist));
            return cut.ToArray();
        }

        private static void Button(SurfaceShapes skirt)
        {
            var radius = Profiles.RadiusAt(Hang, -ButtonBelowTheWaistMetres);
            var front = Math.Sqrt(radius * radius - ButtonRowHalfWidthMetres * ButtonRowHalfWidthMetres) * AskewForm.BodyDepthShare + ButtonProudMetres;
            skirt.Add(Surface.Buttons, Ball.Of(new WorldPoint(-ButtonRowHalfWidthMetres, -ButtonBelowTheWaistMetres, front), ButtonRadiusMetres, AskewForm.FeatureSides));
            skirt.Add(Surface.Buttons, Ball.Of(new WorldPoint(ButtonRowHalfWidthMetres, -ButtonBelowTheWaistMetres, front), ButtonRadiusMetres, AskewForm.FeatureSides));
        }
    }
}
