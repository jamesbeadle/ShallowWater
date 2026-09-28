using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Walking
{
    public sealed class Land
    {
        private readonly CanalPound pound;
        private readonly WaterLines water;
        private readonly Obstacles obstacles;
        private readonly IReadOnlyList<Area> woods;

        public Land(CanalPound pound, WaterLines water, Obstacles obstacles, IReadOnlyList<Area> woods)
        {
            this.pound = pound;
            this.water = water;
            this.obstacles = obstacles;
            this.woods = woods;
        }

        public bool IsOpen(GroundPoint place)
        {
            return !water.IsCovering(place) && !obstacles.IsBlocking(place);
        }

        public GroundPoint Stepped(GroundPoint from, GroundPoint step)
        {
            var candidates = new[] { from + step, from + new GroundPoint(step.East, 0), from + new GroundPoint(0, step.North) };
            var open = candidates.Where(IsOpen).ToList();
            return open.Any() ? open.First() : from;
        }

        public double HeightAt(GroundPoint place)
        {
            var fromTheCanal = Math.Abs(pound.WaterPositionAt(place).Across);
            var isOnTheBanks = fromTheCanal < CanalSection.OuterReachMetres;
            if (isOnTheBanks) return BankProfile.HeightAt(fromTheCanal);
            var isInAWood = woods.Any(wood => wood.IsAround(place));
            return isInAWood ? Heights.WoodlandFloorMetres : Heights.GroundMetres;
        }
    }
}
