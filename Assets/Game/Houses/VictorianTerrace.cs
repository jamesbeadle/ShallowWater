using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class VictorianTerrace : HouseStyle
    {
        private const double NarrowestFrontageMetres = 4.6;
        private const double FrontageSpreadMetres = 0.8;
        private const int FewestHouses = 2;
        private const int MostHouses = 6;
        private const double LowestEavesMetres = 5.0;
        private const double EavesSpreadMetres = 0.5;
        private const double ShareLimewashed = 0.1;
        private const double ShareWithADoorHood = 0.15;

        public override double DepthMetres => 6.4;
        public override double RoofRisePerMetre => 0.7;
        public override double VergeMetres => 0.12;
        public override double StackAboveTheRidgeMetres => 1.0;
        public override double OutshutDepthMetres => 3.0;
        public override Lintel Lintel => Lintel.CambredArch;

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
            var isLimewashed = random.NextDouble() < ShareLimewashed;
            return isLimewashed ? Surface.Limewash : Surface.Wall;
        }

        public override Surface Roof(Random random)
        {
            return Surface.Roof;
        }

        public override Extras ExtrasFor(Random random)
        {
            var hasDoorHood = random.NextDouble() < ShareWithADoorHood;
            return new Extras(hasDoorHood, false, true);
        }

        public override IReadOnlyList<Joinery> FrontOf(House house, double eavesMetres)
        {
            return TerraceFront.Of(house);
        }

        public override IReadOnlyList<Joinery> BackOf(House house, double eavesMetres)
        {
            return TerraceBack.Of(house);
        }

        public override Outshut OutshutOf(House house)
        {
            return TerraceBack.OutshutOf(house);
        }
    }
}
