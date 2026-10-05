using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class OldCottage : HouseStyle
    {
        private const double NarrowestFrontageMetres = 5.2;
        private const double FrontageSpreadMetres = 1.6;
        private const int FewestHouses = 1;
        private const int MostHouses = 3;
        private const double LowestEavesMetres = 3.7;
        private const double EavesSpreadMetres = 0.35;
        private const double ShareLimewashed = 0.5;
        private const double ShareTiled = 0.7;
        private const double ShareWithADoorHood = 0.4;
        private const double ShareWithADormer = 0.6;
        private const double ShareWithAnOutshut = 0.5;
        private const double DeepestSetBackMetres = 0.6;

        public override double DepthMetres => 5.4;
        public override double RoofRisePerMetre => 1.0;
        public override double VergeMetres => 0.3;
        public override double StackAboveTheRidgeMetres => 1.25;
        public override double OutshutDepthMetres => 2.4;
        public override Lintel Lintel => Lintel.SoldierCourse;
        public override bool HasBargeboards => true;

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
            var isTiled = random.NextDouble() < ShareTiled;
            return isTiled ? Surface.RoofTiles : Surface.Roof;
        }

        public override Extras ExtrasFor(Random random)
        {
            var hasDoorHood = random.NextDouble() < ShareWithADoorHood;
            var hasDormer = random.NextDouble() < ShareWithADormer;
            var hasOutshut = random.NextDouble() < ShareWithAnOutshut;
            return new Extras(hasDoorHood, hasDormer, hasOutshut);
        }

        public override double FrontSetBackMetres(Random random)
        {
            return random.NextDouble() * DeepestSetBackMetres;
        }

        public override IReadOnlyList<Joinery> FrontOf(House house, double eavesMetres)
        {
            return CottageFront.Of(house, eavesMetres);
        }

        public override IReadOnlyList<Joinery> DormersOf(House house, double eavesMetres)
        {
            return CottageFront.DormersOf(house, eavesMetres);
        }

        public override IReadOnlyList<Joinery> BackOf(House house, double eavesMetres)
        {
            return CottageBack.Of(house, eavesMetres);
        }

        public override Outshut OutshutOf(House house)
        {
            return CottageBack.OutshutOf(house);
        }
    }
}
