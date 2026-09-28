using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public sealed class Compartments
    {
        public const double SpacingMetres = 230;
        private const double PlantationShare = 0.28;
        private const double DeepInsideMetres = 120;
        private const double SmallestPlantationScale = 0.8;
        private const double PlantationScaleRange = 0.3;
        private const int Seed = 1912;
        private readonly List<Compartment> all = new List<Compartment>();

        public Compartments(Area area, WoodEdge edge)
        {
            var random = new Random(Seed);
            var outline = area.Outline;
            for (var east = outline.West; east < outline.East; east += SpacingMetres)
            {
                for (var north = outline.South; north < outline.North; north += SpacingMetres)
                {
                    var heart = new GroundPoint(east + random.NextDouble() * SpacingMetres, north + random.NextDouble() * SpacingMetres);
                    var isDeepInside = area.IsAround(heart) && edge.DistanceFrom(heart) > DeepInsideMetres;
                    var isPlantation = isDeepInside && random.NextDouble() < PlantationShare;
                    var scale = SmallestPlantationScale + random.NextDouble() * PlantationScaleRange;
                    all.Add(new Compartment(heart, isPlantation, random.NextDouble() * Math.PI, scale));
                }
            }
        }

        public IEnumerable<Compartment> Plantations => all.Where(compartment => compartment.IsPlantation);

        public Compartment Holding(GroundPoint place)
        {
            var holding = all[0];
            var nearest = double.MaxValue;
            foreach (var compartment in all)
            {
                var distance = compartment.Heart.DistanceTo(place);
                if (distance >= nearest) continue;
                nearest = distance;
                holding = compartment;
            }
            return holding;
        }
    }
}
