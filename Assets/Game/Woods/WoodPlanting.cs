using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public static class WoodPlanting
    {
        private const double BroadleafSpacingMetres = 9.5;
        private const double PineSpacingMetres = 6.5;
        private const double BroadleafWander = 0.9;
        private const double PineWanderMetres = 0.7;
        private const double SmallestScale = 0.85;
        private const double YoungestScale = 0.72;
        private const double ScaleRange = 0.3;
        private const double PineScaleJitter = 0.08;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int Seed = 1938;
        private const double Middle = 0.5;

        public static IEnumerable<Tree> Within(Area area)
        {
            var wood = new Wood(area);
            var random = new Random(Seed);
            var trees = BroadleafPlaces(wood.Outline, random).Where(wood.IsBroadleafAt).Select(place => Broadleaf(wood, place, random)).ToList();
            foreach (var plantation in wood.Plantations) trees.AddRange(PinesIn(wood, plantation, random));
            trees.AddRange(Understorey.Within(wood));
            return trees;
        }

        private static Tree Broadleaf(Wood wood, GroundPoint place, Random random)
        {
            var habit = SpeciesMix.HabitAt(place, wood.EdgeDistanceFrom(place), random);
            var scale = (habit.IsYoung ? YoungestScale : SmallestScale) + random.NextDouble() * ScaleRange;
            return new Tree(place, scale, random.NextDouble() * FullTurnRadians, TreeForms.OneOf(habit, random));
        }

        private static Tree Pine(Compartment plantation, GroundPoint place, Random random)
        {
            var scale = plantation.Scale * (1 + (random.NextDouble() - Middle) * PineScaleJitter);
            return new Tree(place, scale, random.NextDouble() * FullTurnRadians, TreeForms.OneOf(PineHabits.Plantation, random));
        }

        private static IEnumerable<Tree> PinesIn(Wood wood, Compartment plantation, Random random)
        {
            var places = PlantationPlaces(plantation, random).Where(place => wood.IsPlantationAt(place, plantation));
            return places.Select(place => Pine(plantation, place, random)).ToList();
        }

        private static IEnumerable<GroundPoint> BroadleafPlaces(GroundRing outline, Random random)
        {
            for (var east = outline.West; east < outline.East; east += BroadleafSpacingMetres)
            {
                for (var north = outline.South; north < outline.North; north += BroadleafSpacingMetres)
                {
                    yield return new GroundPoint(east, north) + Wander(random, BroadleafWander * BroadleafSpacingMetres);
                }
            }
        }

        private static IEnumerable<GroundPoint> PlantationPlaces(Compartment plantation, Random random)
        {
            var along = GroundPoint.Facing(plantation.Bearing) * PineSpacingMetres;
            var across = along.RightAngleClockwise;
            var rows = (int)(Compartments.SpacingMetres / PineSpacingMetres);
            for (var row = -rows; row <= rows; row++)
            {
                for (var column = -rows; column <= rows; column++)
                {
                    yield return plantation.Heart + along * row + across * column + Wander(random, PineWanderMetres);
                }
            }
        }

        private static GroundPoint Wander(Random random, double reachMetres)
        {
            return new GroundPoint(random.NextDouble() - Middle, random.NextDouble() - Middle) * reachMetres;
        }
    }
}
