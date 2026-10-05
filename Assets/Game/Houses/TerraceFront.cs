using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class TerraceFront
    {
        private const double DoorInFromThePartyWallMetres = 0.7;
        private const double DoorWidthMetres = 0.85;
        private const double DoorstepMetres = 0.15;
        private const double DoorHeadMetres = 2.45;
        private const double WindowWidthMetres = 1.0;
        private const double WindowSillMetres = 0.8;
        private const double WindowHeadMetres = 2.2;
        private const double ChamberWindowWidthMetres = 0.95;
        private const double ChamberSillMetres = 3.3;
        private const double ChamberHeadMetres = 4.55;
        private const double LandingWindowWidthMetres = 0.6;
        private const double LandingHeadMetres = 4.35;
        private const double Half = 0.5;

        public static IReadOnlyList<Joinery> Of(House house)
        {
            var paintwork = house.Paintwork;
            var door = house.FromTheDoorSide(DoorInFromThePartyWallMetres);
            var window = house.FromTheFarSide((house.WidthMetres - DoorInFromThePartyWallMetres - DoorWidthMetres * Half) * Half);
            return new List<Joinery>
            {
                Joinery.Door(new Opening(door, DoorWidthMetres, DoorstepMetres, DoorHeadMetres), Glazing.FourPanelDoor, paintwork),
                Joinery.Window(new Opening(window, WindowWidthMetres, WindowSillMetres, WindowHeadMetres), Glazing.TwoOverTwoSash, paintwork),
                Joinery.Window(new Opening(window, ChamberWindowWidthMetres, ChamberSillMetres, ChamberHeadMetres), Glazing.TwoOverTwoSash, paintwork),
                Joinery.Window(new Opening(door, LandingWindowWidthMetres, ChamberSillMetres, LandingHeadMetres), Glazing.TwoOverTwoSash, paintwork),
            };
        }
    }
}
