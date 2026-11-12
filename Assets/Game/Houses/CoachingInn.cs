using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class CoachingInn : HouseStyle
    {
        private const int OneHouse = 1;
        private const double NarrowestFrontageMetres = 12.0;
        private const double FrontageSpreadMetres = 3.0;
        private const double LowestEavesMetres = 6.3;
        private const double EavesSpreadMetres = 0.5;
        private const double ShareLimewashed = 0.6;
        private const double ShareSlated = 0.3;

        public override double DepthMetres => 9.0;
        public override double RoofRisePerMetre => 0.62;
        public override double VergeMetres => 0.25;
        public override double StackAboveTheRidgeMetres => 1.1;
        public override Lintel Lintel => Lintel.CambredArch;
        public override bool StandsAlone => true;

        public override double FrontageMetres(Random random)
        {
            return NarrowestFrontageMetres + random.NextDouble() * FrontageSpreadMetres;
        }

        public override int HousesInARun(Random random)
        {
            return OneHouse;
        }

        public override double EavesMetres(Random random)
        {
            return LowestEavesMetres + random.NextDouble() * EavesSpreadMetres;
        }

        public override Surface Walls(Random random)
        {
            var isLimewashed = random.NextDouble() < ShareLimewashed;
            return isLimewashed ? Surface.Limewash : Surface.Wall;
        }

        public override Surface Roof(Random random)
        {
            var isSlated = random.NextDouble() < ShareSlated;
            return isSlated ? Surface.Roof : Surface.RoofTiles;
        }

        public override Extras ExtrasFor(Random random)
        {
            return new Extras(true, false, false);
        }

        public override IReadOnlyList<Joinery> FrontOf(House house, double eavesMetres)
        {
            return GeorgianElevations.FrontOf(house);
        }

        public override IReadOnlyList<Joinery> BackOf(House house, double eavesMetres)
        {
            return GeorgianElevations.BackOf(house);
        }
    }
}
