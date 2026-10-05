using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class FieldStrips
    {
        public const double HedgeBottomMetres = 1.8;
        public const double FootpathMetres = 1.6;
        public const double HeadlandMetres = 5.5;
        public const double FootpathCentreMetres = HedgeBottomMetres + FootpathMetres / 2;
        private const double AtTheHedge = 0;
        private const double None = 0;

        public static List<FieldStrip>[] Of(Field field)
        {
            var outline = field.Outline;
            var strips = new List<FieldStrip>[outline.EdgeCount];
            for (var edge = 0; edge < outline.EdgeCount; edge++) strips[edge] = Along(field, outline.Boundaries[edge]);
            return strips;
        }

        private static List<FieldStrip> Along(Field field, Boundary boundary)
        {
            var footpath = boundary.HasFootpath ? FootpathMetres : None;
            var headland = field.HasHeadland ? HeadlandMetres : None;
            var footpathEnd = HedgeBottomMetres + footpath;
            var crop = CropSurfaces.Of(field.Crop);
            return new List<FieldStrip>
            {
                new FieldStrip(Surface.FieldMargin, HedgeBottomMetres, AtTheHedge, AtTheHedge),
                new FieldStrip(Surface.Footpath, footpathEnd, HedgeBottomMetres, FootpathCentreMetres),
                new FieldStrip(crop, footpathEnd + headland, footpathEnd, AtTheHedge)
            };
        }
    }
}
