using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class Dressings
    {
        private const double SillOverhangMetres = 0.07;
        private const double SillDepthMetres = 0.08;
        private const double SillProudMetres = 0.06;
        private const double ClearOfTheRevealMetres = 0.01;
        private const double StepOverhangMetres = 0.06;
        private const double StepProudMetres = 0.28;

        public static void Add(SurfaceShapes surfaces, WallFace face, Joinery piece, double setBackMetres)
        {
            var opening = piece.Opening;
            if (piece.IsDoor)
            {
                AddStep(surfaces, face, opening, setBackMetres);
                return;
            }
            var slab = face.Slab(opening.Left - SillOverhangMetres, opening.Right + SillOverhangMetres, -setBackMetres, SillProudMetres);
            var rise = face.Rise;
            var top = rise.FootMetres + opening.Bottom + ClearOfTheRevealMetres;
            surfaces.AddBlock(Surface.Dressing, slab, top - SillDepthMetres, top);
        }

        private static void AddStep(SurfaceShapes surfaces, WallFace face, Opening opening, double setBackMetres)
        {
            var slab = face.Slab(opening.Left - StepOverhangMetres, opening.Right + StepOverhangMetres, -setBackMetres, StepProudMetres);
            var rise = face.Rise;
            surfaces.AddSolid(Surface.Dressing, slab, rise.FootMetres, rise.FootMetres + opening.Bottom + ClearOfTheRevealMetres);
        }
    }
}
