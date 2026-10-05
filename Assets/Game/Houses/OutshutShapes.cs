using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class OutshutShapes
    {
        private const double Half = 0.5;
        private const double AtTheStart = 0;
        private static readonly IReadOnlyList<Joinery> Blank = new List<Joinery>();

        public static void Build(SurfaceShapes surfaces, Run run, Outshut outshut)
        {
            var main = run.Plot;
            var style = run.Style;
            var plot = main.Part(outshut.AlongMetres, main.DepthMetres, outshut.WidthMetres, style.OutshutDepthMetres);
            var rise = new Rise(run.FootMetres, run.FootMetres + outshut.LowEavesMetres);
            var high = run.FootMetres + outshut.HighEavesMetres;
            var sides = PartyWalls(run, outshut);
            JoineryShapes.Fit(surfaces, plot.Back(rise), outshut.Back, run.Walls, run.Lintel);
            RaiseSide(surfaces, run, plot.LeftEnd(rise), sides.IsOnTheLeft ? Blank : FromTheBack(outshut.Side, plot.DepthMetres), high, true);
            RaiseSide(surfaces, run, plot.RightEnd(rise), sides.IsOnTheRight ? Blank : outshut.Side, high, false);
            LeanToRoof.Lay(surfaces, run, plot, new Rise(rise.TopMetres, high), sides);
        }

        private static Neighbours PartyWalls(Run run, Outshut outshut)
        {
            var isFullWidth = outshut.WidthMetres >= run.LengthMetres;
            var isAgainstTheLeft = isFullWidth || !outshut.IsOnTheRight;
            var isAgainstTheRight = isFullWidth || outshut.IsOnTheRight;
            return new Neighbours(isAgainstTheLeft, isAgainstTheRight);
        }

        private static IReadOnlyList<Joinery> FromTheBack(IReadOnlyList<Joinery> side, double lengthMetres)
        {
            return side.Select(piece => RunWalls.SeenFromBehind(piece, lengthMetres)).ToList();
        }

        private static void RaiseSide(SurfaceShapes surfaces, Run run, WallFace side, IReadOnlyList<Joinery> joinery, double highMetres, bool isHighAtTheEnd)
        {
            JoineryShapes.Fit(surfaces, side, joinery, run.Walls, run.Lintel);
            var rise = side.Rise;
            var low = rise.HeightMetres;
            var high = highMetres - rise.FootMetres;
            var length = side.LengthMetres;
            var highAlong = isHighAtTheEnd ? length : AtTheStart;
            var corners = new[] { side.Point(AtTheStart, low), side.Point(length, low), side.Point(highAlong, high) };
            var slope = new Shape();
            Facet.Add(slope, corners, side.SeenFrom(length * Half, low));
            surfaces.Add(run.Walls, slope);
        }
    }
}
