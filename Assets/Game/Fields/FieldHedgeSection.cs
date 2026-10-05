using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class FieldHedgeSection
    {
        private const double HalfWidthMetres = 1;
        private const double FaceMetres = 0.55;
        private const double TopMetres = 0.9;
        private const double FieldHedgeHeightMetres = 1.6;
        private const double FarmHedgeHeightMetres = 2.2;
        private const double RoughnessMetres = 0.35;
        private const double FarmRoughnessMetres = 0.55;
        private const double FromLeftToRight = 1;

        public static IReadOnlyList<Band> Bands(bool isFarmBoundary)
        {
            var top = Heights.GroundMetres + (isFarmBoundary ? FarmHedgeHeightMetres : FieldHedgeHeightMetres);
            var roughness = isFarmBoundary ? FarmRoughnessMetres : RoughnessMetres;
            var walk = new SectionWalk(FromLeftToRight, -HalfWidthMetres, Heights.GroundMetres);
            walk.StepToRough(Surface.Hedge, FaceMetres, top, roughness);
            walk.StepToRough(Surface.Hedge, TopMetres, top, roughness);
            walk.Step(Surface.Hedge, FaceMetres, Heights.GroundMetres);
            return walk.Bands;
        }
    }
}
