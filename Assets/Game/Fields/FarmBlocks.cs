using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public static class FarmBlocks
    {
        private const double SpacingMetres = 620;
        private const double WanderShare = 0.7;
        private const double NeighbourSpacings = 2.5;
        private const double HalfTurnRadians = Math.PI;
        private const double Middle = 0.5;

        public static List<Farm> Across(Farmland farmland, Random random)
        {
            var steadings = Steadings(farmland.Bounds, random);
            var farms = new List<Farm>();
            for (var index = 0; index < steadings.Count; index++)
            {
                var grainBearing = random.NextDouble() * HalfTurnRadians;
                var isNearTheFarmland = farmland.IsNearTheFarmland(steadings[index], SpacingMetres);
                if (!isNearTheFarmland) continue;
                var farm = new Farm(index, OutlineOf(index, steadings, farmland.Bounds), grainBearing);
                if (farm.IsDrawable) farms.Add(farm);
            }
            return farms;
        }

        private static ConvexOutline OutlineOf(int index, List<GroundPoint> steadings, Bounds bounds)
        {
            var steading = steadings[index];
            var outline = ConvexOutline.Rectangle(bounds.West, bounds.East, bounds.South, bounds.North);
            for (var neighbour = 0; neighbour < steadings.Count; neighbour++)
            {
                var other = steadings[neighbour];
                var isNear = neighbour != index && steading.DistanceTo(other) < SpacingMetres * NeighbourSpacings;
                if (!isNear) continue;
                var hedge = Boundary.FarmHedge(FootpathRule.IsHeldBy(index, neighbour), neighbour);
                outline = OutlineCut.KeepRight(outline, CuttingLine.Between(steading, other), hedge);
            }
            return outline;
        }

        private static List<GroundPoint> Steadings(Bounds bounds, Random random)
        {
            var steadings = new List<GroundPoint>();
            for (var east = bounds.West; east < bounds.East; east += SpacingMetres)
            {
                for (var north = bounds.South; north < bounds.North; north += SpacingMetres) steadings.Add(Wandered(east, north, random));
            }
            return steadings;
        }

        private static GroundPoint Wandered(double east, double north, Random random)
        {
            var wander = new GroundPoint(random.NextDouble() - Middle, random.NextDouble() - Middle) * (SpacingMetres * WanderShare);
            return new GroundPoint(east, north) + wander;
        }
    }
}
