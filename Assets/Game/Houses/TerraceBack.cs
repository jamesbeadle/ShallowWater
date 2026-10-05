using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class TerraceBack
    {
        private const double WidestOutshutMetres = 2.7;
        private const double ClearOfTheOutshutMetres = 1.8;
        private const double OutshutLowEavesMetres = 2.3;
        private const double OutshutHighEavesMetres = 3.2;
        private const double KitchenWindowWidthMetres = 0.9;
        private const double KitchenSillMetres = 0.9;
        private const double KitchenHeadMetres = 2.1;
        private const double BedroomWindowWidthMetres = 0.85;
        private const double BedroomSillMetres = 3.4;
        private const double BedroomHeadMetres = 4.5;
        private const double SmallWindowWidthMetres = 0.6;
        private const double SmallWindowSillMetres = 3.6;
        private const double SmallWindowHeadMetres = 4.4;
        private const double SculleryWindowWidthMetres = 0.7;
        private const double SculleryWindowSillMetres = 1.0;
        private const double SculleryWindowHeadMetres = 1.9;
        private const double BackDoorFromTheHouseMetres = 1.4;
        private const double BackDoorWidthMetres = 0.8;
        private const double BackDoorstepMetres = 0.1;
        private const double BackDoorHeadMetres = 2.05;
        private const double Half = 0.5;

        public static IReadOnlyList<Joinery> Of(House house)
        {
            var paintwork = house.Paintwork;
            var outshutWidth = OutshutWidth(house);
            var kitchen = house.FromTheFarSide((house.WidthMetres - outshutWidth) * Half);
            var overTheOutshut = house.FromTheDoorSide(outshutWidth * Half);
            return new List<Joinery>
            {
                Joinery.Window(new Opening(kitchen, KitchenWindowWidthMetres, KitchenSillMetres, KitchenHeadMetres), Glazing.TwoOverTwoSash, paintwork),
                Joinery.Window(new Opening(kitchen, BedroomWindowWidthMetres, BedroomSillMetres, BedroomHeadMetres), Glazing.TwoOverTwoSash, paintwork),
                Joinery.Window(new Opening(overTheOutshut, SmallWindowWidthMetres, SmallWindowSillMetres, SmallWindowHeadMetres), Glazing.FourPanes, paintwork),
            };
        }

        public static Outshut OutshutOf(House house)
        {
            var paintwork = house.Paintwork;
            var width = OutshutWidth(house);
            var middle = house.FromTheDoorSide(width * Half);
            var span = new Opening(middle, width, OutshutLowEavesMetres, OutshutHighEavesMetres);
            var windowOpening = new Opening(width * Half, SculleryWindowWidthMetres, SculleryWindowSillMetres, SculleryWindowHeadMetres);
            var doorOpening = new Opening(BackDoorFromTheHouseMetres, BackDoorWidthMetres, BackDoorstepMetres, BackDoorHeadMetres);
            var window = Joinery.Window(windowOpening, Glazing.FourPanes, paintwork);
            var door = Joinery.Door(doorOpening, Glazing.LedgedDoor, paintwork);
            return new Outshut(span, house.IsDoorOnTheRight, new List<Joinery> { window }, new List<Joinery> { door });
        }

        private static double OutshutWidth(House house)
        {
            return Math.Min(WidestOutshutMetres, house.WidthMetres - ClearOfTheOutshutMetres);
        }
    }
}
