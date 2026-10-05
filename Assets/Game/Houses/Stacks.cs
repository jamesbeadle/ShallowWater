using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class Stacks
    {
        private const double PartyHalfAlongMetres = 0.36;
        private const double PartyHalfAcrossMetres = 0.5;
        private const double EndHalfAlongMetres = 0.3;
        private const double EndHalfAcrossMetres = 0.42;
        private const double InsideTheGableMetres = 0.02;
        private const double HipStackShare = 0.25;
        private const double FootBelowTheRidgeMetres = 1.6;
        private const int PartyPots = 4;
        private const int EndPots = 2;

        public static void Build(SurfaceShapes surfaces, Run run)
        {
            foreach (var stack in Placed(run)) Raise(surfaces, run, stack);
        }

        private static IEnumerable<StackPlace> Placed(Run run)
        {
            var length = run.LengthMetres;
            if (run.IsHipped)
            {
                yield return new StackPlace(length * HipStackShare, EndHalfAlongMetres, EndHalfAcrossMetres, EndPots);
                yield return new StackPlace(length * (1 - HipStackShare), EndHalfAlongMetres, EndHalfAcrossMetres, EndPots);
                yield break;
            }
            var inset = EndHalfAlongMetres + InsideTheGableMetres;
            yield return new StackPlace(inset, EndHalfAlongMetres, EndHalfAcrossMetres, EndPots);
            foreach (var wall in run.PartyWalls()) yield return new StackPlace(wall, PartyHalfAlongMetres, PartyHalfAcrossMetres, PartyPots);
            var neighbours = run.Neighbours;
            if (neighbours.IsOnTheRight) yield break;
            yield return new StackPlace(length - inset, EndHalfAlongMetres, EndHalfAcrossMetres, EndPots);
        }

        private static void Raise(SurfaceShapes surfaces, Run run, StackPlace stack)
        {
            var plot = run.Plot;
            var style = run.Style;
            var middle = run.HalfDepthMetres;
            var foot = run.RidgeMetres - FootBelowTheRidgeMetres;
            var top = run.RidgeMetres + style.StackAboveTheRidgeMetres;
            var shaft = plot.Oblong(stack.AlongMetres, middle, stack.HalfAlongMetres, stack.HalfAcrossMetres);
            surfaces.AddBlock(Surface.Brickwork, shaft, foot, top - StackCap.DepthMetres);
            StackCap.Add(surfaces, plot, stack, middle, top);
            ChimneyPots.Set(surfaces, plot, stack, middle, top);
        }
    }
}
