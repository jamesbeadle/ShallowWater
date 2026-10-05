using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class DormerShapes
    {
        private const double CheekBesideTheWindowMetres = 0.28;
        private const double EavesAboveTheHeadMetres = 0.28;
        private const double RisePerMetre = 1.0;
        private const double Half = 0.5;
        private const double AtTheStart = 0;
        private static readonly double[] Cheeks = { -1, 1 };

        public static void Build(SurfaceShapes surfaces, Run run, Joinery window)
        {
            var opening = window.Opening;
            var halfWidth = opening.WidthMetres * Half + CheekBesideTheWindowMetres;
            var eaves = run.FootMetres + opening.Top + EavesAboveTheHeadMetres;
            var dormer = new Dormer(run, opening.CentreAlong, halfWidth, eaves, RisePerMetre);
            var face = dormer.Front();
            var belowTheMainEaves = run.FootMetres - run.EavesMetres;
            var inTheFace = new Opening(halfWidth, opening.WidthMetres, opening.Bottom + belowTheMainEaves, opening.Top + belowTheMainEaves);
            var framed = new Joinery(window.Kind, inTheFace, window.Pattern, window.Paint);
            JoineryShapes.Fit(surfaces, face, new List<Joinery> { framed }, run.Walls, run.Lintel);
            surfaces.Add(run.Walls, Gable(face, dormer));
            foreach (var cheek in Cheeks) surfaces.Add(run.Roof, dormer.Cheek(cheek));
            foreach (var cheek in Cheeks) DormerRoof.Lay(surfaces, run, dormer, cheek);
        }

        private static Shape Gable(WallFace face, Dormer dormer)
        {
            var eaves = face.HeightMetres;
            var rise = face.Rise;
            var ridge = dormer.RidgeMetres - rise.FootMetres;
            var width = face.LengthMetres;
            var corners = new[] { face.Point(AtTheStart, eaves), face.Point(width, eaves), face.Point(width * Half, ridge) };
            var gable = new Shape();
            Facet.Add(gable, corners, face.SeenFrom(width * Half, eaves));
            return gable;
        }
    }
}
