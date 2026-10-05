using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class GapFurnishing
    {
        private const double Shut = 0;
        private const double LeastSwingRadians = 1.2;
        private const double SwingRangeRadians = 0.6;
        private const double EitherWay = 0.5;
        private const double Halfway = 0.5;

        public static void Add(SurfaceShapes surfaces, Hedgerow hedgerow, HedgeGap gap, Farmland farmland, Random random)
        {
            var from = hedgerow.At(gap.FromMetres);
            var to = hedgerow.At(gap.ToMetres);
            var middle = hedgerow.At(gap.FromMetres + gap.WidthMetres * Halfway);
            var swing = Swing(random);
            if (!farmland.IsHedged(middle)) return;
            if (gap.Furniture == GapFurniture.Stile) Stile.Add(surfaces, from, to);
            if (gap.Furniture == GapFurniture.ShutGate) FieldGate.Add(surfaces, from, to, Shut);
            if (gap.Furniture == GapFurniture.OpenGate) FieldGate.Add(surfaces, from, to, swing);
        }

        private static double Swing(Random random)
        {
            var side = random.NextDouble() < EitherWay ? 1 : -1;
            return side * (LeastSwingRadians + random.NextDouble() * SwingRangeRadians);
        }
    }
}
