using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Pound
{
    public static class PoundShapes
    {
        public static SurfaceShapes Of(Pound pound)
        {
            var surfaces = new SurfaceShapes();
            foreach (var stretch in PoundStretches.Of(pound)) Dig(surfaces, pound, stretch);
            return surfaces;
        }

        private static void Dig(SurfaceShapes surfaces, Pound pound, Stretch stretch)
        {
            var line = pound.LineAlong(stretch.Span);
            foreach (var band in CanalSection.For(stretch)) surfaces.AddRibbon(line, band);
            if (stretch.IsMooring) Bollards.StandAlong(surfaces, pound, stretch);
        }
    }
}
