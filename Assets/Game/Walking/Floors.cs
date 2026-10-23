using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class Floors
    {
        private const double CellMetres = 40;
        private readonly PlaceGrid<FloorSlab> slabs = new PlaceGrid<FloorSlab>(CellMetres);
        private readonly PlaceGrid<FloorStrip> strips = new PlaceGrid<FloorStrip>(CellMetres);

        public void AddSlab(GroundRing outline, double heightMetres)
        {
            var slab = new FloorSlab(outline, heightMetres);
            slabs.Add(slab, slab.SouthWest, slab.NorthEast);
        }

        public void AddStrips(IEnumerable<FloorStrip> laid)
        {
            foreach (var strip in laid)
            {
                var reach = strip.Reach;
                strips.Add(strip, reach.SouthWest, reach.NorthEast);
            }
        }

        public double? HeightUnder(GroundPoint place)
        {
            foreach (var slab in slabs.Near(place))
            {
                if (slab.IsUnder(place)) return slab.HeightMetres;
            }
            foreach (var strip in strips.Near(place))
            {
                if (strip.IsUnder(place)) return strip.HeightMetres;
            }
            return null;
        }
    }
}
