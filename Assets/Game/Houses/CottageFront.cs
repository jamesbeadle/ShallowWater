using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class CottageFront
    {
        private const double DoorInFromThePartyWallMetres = 1.1;
        private const double DoorWidthMetres = 0.8;
        private const double DoorstepMetres = 0.1;
        private const double DoorHeadMetres = 2.0;
        private const double WindowWidthMetres = 1.1;
        private const double WindowSillMetres = 0.9;
        private const double WindowHeadMetres = 1.9;
        private const double UpperWindowWidthMetres = 0.8;
        private const double UpperSillBelowTheEavesMetres = 1.0;
        private const double UpperHeadBelowTheEavesMetres = 0.33;
        private const double DormerWindowWidthMetres = 0.85;
        private const double DormerSillAboveTheEavesMetres = 0.1;
        private const double DormerHeadAboveTheEavesMetres = 0.95;
        private const double Half = 0.5;
        private static readonly IReadOnlyList<Joinery> None = new List<Joinery>();

        public static IReadOnlyList<Joinery> Of(House house, double eavesMetres)
        {
            var paintwork = house.Paintwork;
            var door = house.FromTheDoorSide(DoorInFromThePartyWallMetres);
            var window = WindowAlong(house);
            var front = new List<Joinery>
            {
                Joinery.Door(new Opening(door, DoorWidthMetres, DoorstepMetres, DoorHeadMetres), Glazing.LedgedDoor, paintwork),
                Joinery.Window(new Opening(window, WindowWidthMetres, WindowSillMetres, WindowHeadMetres), Glazing.Casement, paintwork),
            };
            var extras = house.Extras;
            if (extras.HasDormer) return front;
            var upper = new Opening(window, UpperWindowWidthMetres, eavesMetres - UpperSillBelowTheEavesMetres, eavesMetres - UpperHeadBelowTheEavesMetres);
            front.Add(Joinery.Window(upper, Glazing.Casement, paintwork));
            return front;
        }

        public static IReadOnlyList<Joinery> DormersOf(House house, double eavesMetres)
        {
            var extras = house.Extras;
            if (!extras.HasDormer) return None;
            var sill = eavesMetres + DormerSillAboveTheEavesMetres;
            var opening = new Opening(WindowAlong(house), DormerWindowWidthMetres, sill, eavesMetres + DormerHeadAboveTheEavesMetres);
            return new List<Joinery> { Joinery.Window(opening, Glazing.Casement, house.Paintwork) };
        }

        private static double WindowAlong(House house)
        {
            var doorFarEdge = DoorInFromThePartyWallMetres + DoorWidthMetres * Half;
            return house.FromTheFarSide((house.WidthMetres - doorFarEdge) * Half);
        }
    }
}
