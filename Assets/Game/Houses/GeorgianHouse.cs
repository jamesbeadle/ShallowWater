using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class GeorgianHouse : HouseStyle
    {
        private const double NarrowestFrontageMetres = 9.0;
        private const double FrontageSpreadMetres = 1.4;
        private const int FewestHouses = 1;
        private const int MostHouses = 2;
        private const double LowestEavesMetres = 6.1;
        private const double EavesSpreadMetres = 0.4;
        private const double ShareStuccoed = 0.25;
        private const double DeepestSetBackMetres = 1.2;

        public override double DepthMetres => 8.0;
        public override double RoofRisePerMetre => 0.55;
        public override double VergeMetres => 0.2;
        public override double StackAboveTheRidgeMetres => 1.0;
        public override Lintel Lintel => Lintel.StoneWithKeystone;
        public override bool CanBeHipped => true;
        public override bool StandsAlone => true;

        public override double FrontageMetres(Random random)
        {
            return NarrowestFrontageMetres + random.NextDouble() * FrontageSpreadMetres;
        }

        public override int HousesInARun(Random random)
        {
            return random.Next(FewestHouses, MostHouses + 1);
        }

        public override double EavesMetres(Random random)
        {
            return LowestEavesMetres + random.NextDouble() * EavesSpreadMetres;
        }

        public override Surface Walls(Random random)
        {
            var isStuccoed = random.NextDouble() < ShareStuccoed;
            return isStuccoed ? Surface.Limewash : Surface.Wall;
        }

        public override Surface Roof(Random random)
        {
            return Surface.Roof;
        }

        public override Extras ExtrasFor(Random random)
        {
            return new Extras(true, false, false);
        }

        public override double FrontSetBackMetres(Random random)
        {
            return random.NextDouble() * DeepestSetBackMetres;
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
