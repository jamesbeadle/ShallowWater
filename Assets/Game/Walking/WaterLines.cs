using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class WaterLines
    {
        private const double CellMetres = 60;
        private readonly PlaceGrid<WaterReach> reaches = new PlaceGrid<WaterReach>(CellMetres);

        public void Add(GroundLine line, double halfWidthMetres)
        {
            for (var segment = 0; segment < line.SegmentCount; segment++)
            {
                var from = line[segment];
                var to = line[segment + 1];
                var margin = new GroundPoint(halfWidthMetres, halfWidthMetres);
                var southWest = new GroundPoint(Math.Min(from.East, to.East), Math.Min(from.North, to.North));
                var northEast = new GroundPoint(Math.Max(from.East, to.East), Math.Max(from.North, to.North));
                reaches.Add(new WaterReach(from, to, halfWidthMetres), southWest - margin, northEast + margin);
            }
        }

        public bool IsCovering(GroundPoint place)
        {
            return reaches.HasAnyNear(place, reach => reach.IsCovering(place));
        }
    }
}
