using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Recess
    {
        private const double Half = 0.5;

        public static Shape Reveals(WallFace face, Opening opening, double depthMetres)
        {
            var reveals = new Shape();
            var back = face.Inset(depthMetres);
            var halfway = face.Inset(depthMetres * Half);
            var viewer = halfway.Point(opening.CentreAlong, opening.MiddleHeight);
            AddReveal(reveals, face, back, (opening.Left, opening.Bottom), (opening.Left, opening.Top), viewer);
            AddReveal(reveals, face, back, (opening.Right, opening.Bottom), (opening.Right, opening.Top), viewer);
            AddReveal(reveals, face, back, (opening.Left, opening.Top), (opening.Right, opening.Top), viewer);
            AddReveal(reveals, face, back, (opening.Left, opening.Bottom), (opening.Right, opening.Bottom), viewer);
            return reveals;
        }

        public static Shape Filling(WallFace face, Opening opening, double depthMetres, Fitting fitting)
        {
            var filling = new Shape();
            var back = face.Inset(depthMetres);
            back.AddRectangle(filling, opening, fitting);
            return filling;
        }

        private static void AddReveal(Shape reveals, WallFace face, WallFace back, (double along, double up) from, (double along, double up) to, WorldPoint eye)
        {
            var corners = new List<WorldPoint>
            {
                face.Point(from.along, from.up), back.Point(from.along, from.up), back.Point(to.along, to.up), face.Point(to.along, to.up),
            };
            var start = new SurfacePlace(from.along, from.up);
            var end = new SurfacePlace(to.along, to.up);
            var places = new List<SurfacePlace> { start, start, end, end };
            Facet.Add(reveals, corners, places, eye);
        }
    }
}
