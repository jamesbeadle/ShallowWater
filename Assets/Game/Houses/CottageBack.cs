using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class CottageBack
    {
        private const double WindowWidthMetres = 0.7;
        private const double WindowSillMetres = 1.0;
        private const double WindowHeadMetres = 1.8;
        private const double UpperWindowWidthMetres = 0.6;
        private const double UpperSillBelowTheEavesMetres = 0.9;
        private const double UpperHeadBelowTheEavesMetres = 0.33;
        private const double OutshutLowEavesMetres = 2.15;
        private const double OutshutRiseMetres = 0.65;
        private const double OutshutWindowHeadMetres = 1.75;
        private const double OutshutWindowAcross = 0.68;
        private const double BackDoorAcross = 0.25;
        private const double BackDoorWidthMetres = 0.75;
        private const double BackDoorstepMetres = 0.1;
        private const double BackDoorHeadMetres = 1.85;

        public static IReadOnlyList<Joinery> Of(House house, double eavesMetres)
        {
            var paintwork = house.Paintwork;
            var extras = house.Extras;
            var back = new List<Joinery>();
            var sill = eavesMetres - UpperSillBelowTheEavesMetres;
            var upper = new Opening(house.MiddleMetres, UpperWindowWidthMetres, sill, eavesMetres - UpperHeadBelowTheEavesMetres);
            back.Add(Joinery.Window(upper, Glazing.Casement, paintwork));
            if (extras.HasOutshut) return back;
            var ground = new Opening(house.MiddleMetres, WindowWidthMetres, WindowSillMetres, WindowHeadMetres);
            back.Add(Joinery.Window(ground, Glazing.Casement, paintwork));
            return back;
        }

        public static Outshut OutshutOf(House house)
        {
            var paintwork = house.Paintwork;
            var width = house.WidthMetres;
            var span = new Opening(house.MiddleMetres, width, OutshutLowEavesMetres, OutshutLowEavesMetres + OutshutRiseMetres);
            var windowOpening = new Opening(width * OutshutWindowAcross, WindowWidthMetres, WindowSillMetres, OutshutWindowHeadMetres);
            var doorOpening = new Opening(width * BackDoorAcross, BackDoorWidthMetres, BackDoorstepMetres, BackDoorHeadMetres);
            var window = Joinery.Window(windowOpening, Glazing.Casement, paintwork);
            var door = Joinery.Door(doorOpening, Glazing.LedgedDoor, paintwork);
            return new Outshut(span, house.IsDoorOnTheRight, new List<Joinery> { window, door }, new List<Joinery>());
        }
    }
}
