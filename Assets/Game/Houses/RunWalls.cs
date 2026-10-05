using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class RunWalls
    {
        private const double Half = 0.5;
        private static readonly IReadOnlyList<Joinery> Blank = new List<Joinery>();

        public static void Raise(SurfaceShapes surfaces, Run run)
        {
            var plot = run.Plot;
            var style = run.Style;
            var storeys = run.Storeys;
            var eaves = run.EavesAboveTheGroundMetres;
            var walls = run.Walls;
            var front = run.Houses.SelectMany(house => style.FrontOf(house, eaves)).ToList();
            var back = run.Houses.SelectMany(house => style.BackOf(house, eaves)).Select(piece => SeenFromBehind(piece, plot.LengthMetres)).ToList();
            JoineryShapes.Fit(surfaces, plot.Front(storeys), front, walls, run.Lintel);
            JoineryShapes.Fit(surfaces, plot.Back(storeys), back, walls, run.Lintel);
            RaiseEnd(surfaces, run, plot.LeftEnd(storeys));
            RaiseEnd(surfaces, run, plot.RightEnd(storeys));
        }

        public static Joinery SeenFromBehind(Joinery piece, double lengthMetres)
        {
            var opening = piece.Opening;
            var mirrored = new Opening(lengthMetres - opening.CentreAlong, opening.WidthMetres, opening.Bottom, opening.Top);
            return new Joinery(piece.Kind, mirrored, piece.Pattern, piece.Paint);
        }

        private static void RaiseEnd(SurfaceShapes surfaces, Run run, WallFace end)
        {
            var walls = run.Walls;
            JoineryShapes.Fit(surfaces, end, Blank, walls, run.Lintel);
            if (run.IsHipped) return;
            surfaces.Add(walls, Gable(end, run.RidgeMetres));
        }

        private static Shape Gable(WallFace end, double ridgeMetres)
        {
            var rise = end.Rise;
            var eaves = rise.HeightMetres;
            var ridge = ridgeMetres - rise.FootMetres;
            var length = end.LengthMetres;
            var corners = new[] { end.Point(0, eaves), end.Point(length, eaves), end.Point(length * Half, ridge) };
            var places = new[] { new SurfacePlace(0, eaves), new SurfacePlace(length, eaves), new SurfacePlace(length * Half, ridge) };
            var gable = new Shape();
            Facet.Add(gable, corners, places, end.SeenFrom(length * Half, eaves));
            return gable;
        }
    }
}
