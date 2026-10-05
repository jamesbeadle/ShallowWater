using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public sealed class HedgerowTiles
    {
        private const double TileMetres = 1000;
        private const double Halfway = 0.5;

        private readonly Dictionary<(long, long), SurfaceShapes> tiles = new Dictionary<(long, long), SurfaceShapes>();

        public IEnumerable<SurfaceShapes> All => tiles.Values;

        public SurfaceShapes TileOf(Hedgerow hedgerow)
        {
            var middle = hedgerow.At(hedgerow.LengthMetres * Halfway);
            var key = (Cell(middle.East), Cell(middle.North));
            var isKnown = tiles.TryGetValue(key, out var tile);
            if (isKnown) return tile;
            tile = new SurfaceShapes();
            tiles[key] = tile;
            return tile;
        }

        private static long Cell(double metres)
        {
            return (long)Math.Floor(metres / TileMetres);
        }
    }
}
