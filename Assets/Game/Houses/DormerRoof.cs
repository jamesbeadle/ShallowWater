using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class DormerRoof
    {
        private const double ForwardOverhangMetres = 0.14;
        private const double SideOverhangMetres = 0.1;
        private const double SoffitBelowMetres = 0.05;
        private const double BargeboardMetres = 0.16;
        private const double ViewingHeightMetres = 3;
        private const double FirstSide = -1;
        private const double AtTheEaves = 0;
        private const double Flush = 0;

        public static void Lay(SurfaceShapes surfaces, Run run, Dormer dormer, double side)
        {
            surfaces.Add(run.Roof, Plane(dormer, side, Flush, true));
            surfaces.Add(Surface.Timber, Plane(dormer, side, SoffitBelowMetres, false));
            surfaces.Add(Surface.Timber, Bargeboard(dormer, side));
            if (side != FirstSide) return;
            var plot = dormer.Plot;
            var ridge = dormer.RidgeMetres;
            var front = plot.Point(dormer.CentreMetres, -ForwardOverhangMetres, ridge);
            var back = plot.Point(dormer.CentreMetres, dormer.BackWhereTheRoofMeets(ridge), ridge);
            RidgeTiles.Lay(surfaces, front, back, plot.Lengthways);
        }

        private static Shape Plane(Dormer dormer, double side, double belowMetres, bool isFacingUp)
        {
            var plot = dormer.Plot;
            var along = dormer.CentreMetres + side * (dormer.HalfWidthMetres + SideOverhangMetres);
            var eaves = dormer.EavesMetres - SideOverhangMetres * dormer.RisePerMetre - belowMetres;
            var ridge = dormer.RidgeMetres - belowMetres;
            var eavesBack = dormer.BackWhereTheRoofMeets(eaves);
            var ridgeBack = dormer.BackWhereTheRoofMeets(ridge);
            var up = (dormer.HalfWidthMetres + SideOverhangMetres) * Math.Sqrt(1 + dormer.RisePerMetre * dormer.RisePerMetre);
            var corners = new[]
            {
                plot.Point(along, -ForwardOverhangMetres, eaves), plot.Point(dormer.CentreMetres, -ForwardOverhangMetres, ridge),
                plot.Point(dormer.CentreMetres, ridgeBack, ridge), plot.Point(along, eavesBack, eaves),
            };
            var front = -ForwardOverhangMetres;
            var places = new[]
            {
                new SurfacePlace(front, AtTheEaves), new SurfacePlace(front, up), new SurfacePlace(ridgeBack, up), new SurfacePlace(eavesBack, AtTheEaves),
            };
            var lookingFrom = isFacingUp ? ViewingHeightMetres : -ViewingHeightMetres;
            var plane = new Shape();
            Facet.Add(plane, corners, places, plot.Point(along, eavesBack, eaves + lookingFrom));
            return plane;
        }

        private static Shape Bargeboard(Dormer dormer, double side)
        {
            var plot = dormer.Plot;
            var along = dormer.CentreMetres + side * (dormer.HalfWidthMetres + SideOverhangMetres);
            var eaves = dormer.EavesMetres - SideOverhangMetres * dormer.RisePerMetre;
            var ridge = dormer.RidgeMetres;
            var front = -ForwardOverhangMetres;
            var corners = new[]
            {
                plot.Point(along, front, eaves), plot.Point(dormer.CentreMetres, front, ridge),
                plot.Point(dormer.CentreMetres, front, ridge - BargeboardMetres), plot.Point(along, front, eaves - BargeboardMetres),
            };
            var board = new Shape();
            Facet.Add(board, corners, plot.Point(dormer.CentreMetres, front - ViewingHeightMetres, eaves));
            return board;
        }
    }
}
