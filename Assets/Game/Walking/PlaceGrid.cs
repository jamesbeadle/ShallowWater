using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Walking
{
    public sealed class PlaceGrid<T>
    {
        private static readonly IReadOnlyList<T> Nothing = new T[0];
        private readonly Dictionary<(long, long), List<T>> cells = new Dictionary<(long, long), List<T>>();
        private readonly double cellMetres;

        public PlaceGrid(double cellMetres)
        {
            this.cellMetres = cellMetres;
        }

        public void Add(T item, GroundPoint southWest, GroundPoint northEast)
        {
            for (var east = Cell(southWest.East); east <= Cell(northEast.East); east++)
            {
                for (var north = Cell(southWest.North); north <= Cell(northEast.North); north++) CellAt(east, north).Add(item);
            }
        }

        public IReadOnlyList<T> Near(GroundPoint place)
        {
            var isKnown = cells.TryGetValue((Cell(place.East), Cell(place.North)), out var items);
            return isKnown ? items : Nothing;
        }

        public bool HasAnyNear(GroundPoint place, Func<T, bool> isTrue)
        {
            return Near(place).Any(isTrue);
        }

        private long Cell(double metres)
        {
            return (long)Math.Floor(metres / cellMetres);
        }

        private List<T> CellAt(long east, long north)
        {
            var isKnown = cells.TryGetValue((east, north), out var items);
            if (isKnown) return items;
            items = new List<T>();
            cells[(east, north)] = items;
            return items;
        }
    }
}
