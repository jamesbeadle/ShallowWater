using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewScarf
    {
        private const double WrapRadiusMetres = 0.078;
        private const double WrapMetres = AskewForm.NeckAboveTheHips + 0.005;
        private const double ThicknessMetres = 0.028;
        private const int WrapCorners = 14;
        private const double FullTurnRadians = 2 * Math.PI;

        private static readonly WorldPoint[] Tail =
        {
            new WorldPoint(0.035, WrapMetres - 0.02, 0.11), new WorldPoint(0.025, WrapMetres - 0.1, 0.172),
            new WorldPoint(-0.005, WrapMetres - 0.2, 0.172), new WorldPoint(-0.025, WrapMetres - 0.27, 0.168)
        };

        public static void Wrap(SurfaceShapes torso)
        {
            var loop = Enumerable.Range(0, WrapCorners).Select(corner =>
            {
                var around = FullTurnRadians * corner / WrapCorners;
                return new WorldPoint(WrapRadiusMetres * Math.Cos(around), WrapMetres, WrapRadiusMetres * Math.Sin(around));
            }).ToList();
            torso.Add(Surface.Scarf, Tube.Around(loop, ThicknessMetres, AskewForm.FeatureSides));
            torso.Add(Surface.Scarf, Tube.Along(Tail, ThicknessMetres, AskewForm.FeatureSides));
        }
    }
}
