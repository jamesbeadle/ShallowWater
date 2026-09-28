using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class Obstacles
    {
        private const double FootprintCellMetres = 40;
        private const double TrunkCellMetres = 8;
        private readonly PlaceGrid<GroundRing> footprints = new PlaceGrid<GroundRing>(FootprintCellMetres);
        private readonly PlaceGrid<Trunk> trunks = new PlaceGrid<Trunk>(TrunkCellMetres);

        public void AddFootprints(IEnumerable<GroundRing> rings)
        {
            var margin = new GroundPoint(WalkingPace.BodyRadiusMetres, WalkingPace.BodyRadiusMetres);
            foreach (var ring in rings)
            {
                footprints.Add(ring, new GroundPoint(ring.West, ring.South) - margin, new GroundPoint(ring.East, ring.North) + margin);
            }
        }

        public void AddTrunks(IEnumerable<Trunk> standing)
        {
            foreach (var trunk in standing)
            {
                var reach = trunk.RadiusMetres + WalkingPace.BodyRadiusMetres;
                var margin = new GroundPoint(reach, reach);
                trunks.Add(trunk, trunk.Foot - margin, trunk.Foot + margin);
            }
        }

        public bool IsBlocking(GroundPoint place)
        {
            var isAgainstAWall = footprints.HasAnyNear(place, footprint => Walls.IsBlocking(footprint, place));
            return isAgainstAWall || trunks.HasAnyNear(place, trunk => trunk.IsBlocking(place));
        }
    }
}
