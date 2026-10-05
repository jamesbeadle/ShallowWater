using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class GeorgianElevations
    {
        private const double BayShareOfTheFrontage = 0.29;
        private const double DoorWidthMetres = 1.0;
        private const double DoorstepMetres = 0.2;
        private const double DoorHeadMetres = 2.9;
        private const double WindowWidthMetres = 1.1;
        private const double GroundSillMetres = 0.75;
        private const double GroundHeadMetres = 2.55;
        private const double UpperSillMetres = 3.7;
        private const double UpperHeadMetres = 5.25;
        private const double BackWindowWidthMetres = 1.0;
        private const double StairWindowSillMetres = 2.4;
        private const double StairWindowHeadMetres = 4.6;
        private static readonly double[] Bays = { -1, 1 };

        public static IReadOnlyList<Joinery> FrontOf(House house)
        {
            var paintwork = house.Paintwork;
            var middle = house.MiddleMetres;
            var bays = BayCentres(house);
            var doorway = new Opening(middle, DoorWidthMetres, DoorstepMetres, DoorHeadMetres);
            var front = new List<Joinery> { Joinery.Door(doorway, Glazing.SixPanelDoor, paintwork) };
            front.AddRange(bays.Select(bay => Sash(bay, WindowWidthMetres, GroundSillMetres, GroundHeadMetres, paintwork)));
            front.AddRange(bays.Append(middle).Select(bay => Sash(bay, WindowWidthMetres, UpperSillMetres, UpperHeadMetres, paintwork)));
            return front;
        }

        public static IReadOnlyList<Joinery> BackOf(House house)
        {
            var paintwork = house.Paintwork;
            var bays = BayCentres(house);
            var back = bays.Select(bay => Sash(bay, BackWindowWidthMetres, GroundSillMetres, GroundHeadMetres, paintwork)).ToList();
            back.AddRange(bays.Select(bay => Sash(bay, BackWindowWidthMetres, UpperSillMetres, UpperHeadMetres, paintwork)));
            back.Add(Sash(house.MiddleMetres, BackWindowWidthMetres, StairWindowSillMetres, StairWindowHeadMetres, paintwork));
            return back;
        }

        private static List<double> BayCentres(House house)
        {
            var middle = house.MiddleMetres;
            var spacing = house.WidthMetres * BayShareOfTheFrontage;
            return Bays.Select(side => middle + side * spacing).ToList();
        }

        private static Joinery Sash(double along, double widthMetres, double sillMetres, double headMetres, Paintwork paintwork)
        {
            return Joinery.Window(new Opening(along, widthMetres, sillMetres, headMetres), Glazing.SixOverSixSash, paintwork);
        }
    }
}
