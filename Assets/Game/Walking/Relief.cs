using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class Relief
    {
        private readonly IReadOnlyList<Area> woods;
        private readonly IReadOnlyList<Area> villages;

        private readonly Floors floors;

        public Relief(IReadOnlyList<Area> woods, IReadOnlyList<Area> villages, Floors floors)
        {
            this.woods = woods;
            this.villages = villages;
            this.floors = floors;
        }

        public double HeightAt(GroundPoint place)
        {
            var floor = floors.HeightUnder(place);
            if (floor.HasValue) return floor.Value;
            var isInAWood = woods.Any(wood => wood.IsAround(place));
            if (isInAWood) return Heights.WoodlandFloorMetres;
            var isInAVillage = villages.Any(village => village.IsAround(place));
            return isInAVillage ? Heights.VillageTurfMetres : Heights.GroundMetres;
        }
    }
}
