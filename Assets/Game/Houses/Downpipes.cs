using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class Downpipes
    {
        private const double InFromTheEndMetres = 0.1;
        private const double WidthMetres = 0.075;
        private const double OffTheWallMetres = 0.02;
        private const double BelowTheGutterMetres = 0.07;
        private const int PartyWallsBetweenPipes = 2;

        public static void Fix(SurfaceShapes surfaces, Run run)
        {
            var plot = run.Plot;
            var pitch = Pitch.Of(run);
            var top = Gutters.BottomOf(pitch);
            var reach = RoofForm.EavesOverhangMetres + Gutters.OutFromTheEavesLine;
            var storeys = new Rise(run.FootMetres, top);
            foreach (var along in Places(run))
            {
                Fix(surfaces, plot.Front(storeys), along, reach);
                Fix(surfaces, plot.Back(storeys), run.LengthMetres - along, reach);
            }
        }

        private static IEnumerable<double> Places(Run run)
        {
            yield return InFromTheEndMetres;
            var walls = run.PartyWalls().ToList();
            for (var wall = 1; wall < walls.Count; wall += PartyWallsBetweenPipes) yield return walls[wall];
            var neighbours = run.Neighbours;
            if (!neighbours.IsOnTheRight) yield return run.LengthMetres - InFromTheEndMetres;
        }

        private static void Fix(SurfaceShapes surfaces, WallFace face, double along, double reachMetres)
        {
            var rise = face.Rise;
            var left = along - WidthMetres / 2;
            var right = along + WidthMetres / 2;
            var pipe = face.Slab(left, right, OffTheWallMetres, OffTheWallMetres + WidthMetres);
            surfaces.AddBlock(Surface.Iron, pipe, rise.FootMetres, rise.TopMetres - BelowTheGutterMetres);
            var neck = face.Slab(left, right, OffTheWallMetres, reachMetres);
            surfaces.AddBlock(Surface.Iron, neck, rise.TopMetres - BelowTheGutterMetres, rise.TopMetres);
        }
    }
}
