using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class RunShapes
    {
        public static void Raise(SurfaceShapes surfaces, Run run)
        {
            RunWalls.Raise(surfaces, run);
            Roof(surfaces, run);
            Stacks.Build(surfaces, run);
            Downpipes.Fix(surfaces, run);
            foreach (var house in run.Houses) Finish(surfaces, run, house);
        }

        private static void Roof(SurfaceShapes surfaces, Run run)
        {
            if (run.IsHipped)
            {
                HippedRoof.Lay(surfaces, run);
                return;
            }
            GabledRoof.Lay(surfaces, run);
        }

        private static void Finish(SurfaceShapes surfaces, Run run, House house)
        {
            var style = run.Style;
            var extras = house.Extras;
            var eaves = run.EavesAboveTheGroundMetres;
            foreach (var window in style.DormersOf(house, eaves)) DormerShapes.Build(surfaces, run, window);
            var outshut = style.OutshutOf(house);
            if (extras.HasOutshut && outshut.IsBuilt) OutshutShapes.Build(surfaces, run, outshut);
            if (!extras.HasDoorHood) return;
            var plot = run.Plot;
            var door = style.FrontOf(house, eaves).First(piece => piece.IsDoor);
            DoorHood.Hang(surfaces, plot.Front(run.Storeys), door.Opening);
        }
    }
}
