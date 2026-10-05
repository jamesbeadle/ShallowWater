using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class DistanceGrid
    {
        private const double CellMetres = 25;
        private const double Halfway = 0.5;

        private readonly double west;
        private readonly double south;
        private readonly int columns;
        private readonly int rows;
        private readonly NearestSamples nearest;

        public DistanceGrid(Bounds bounds, IEnumerable<GroundLine> lines)
        {
            west = bounds.West;
            south = bounds.South;
            columns = (int)Math.Ceiling(bounds.WidthMetres / CellMetres) + 1;
            rows = (int)Math.Ceiling(bounds.DepthMetres / CellMetres) + 1;
            nearest = new NearestSamples(columns, rows, CentreOf);
            foreach (var line in lines) Seed(line);
            nearest.Spread();
        }

        public double At(GroundPoint place)
        {
            var column = ColumnOf(place);
            var row = RowOf(place);
            var isOutside = column < 0 || row < 0 || column >= columns || row >= rows;
            if (isOutside) return double.MaxValue;
            return nearest.DistanceAt(column, row);
        }

        private void Seed(GroundLine line)
        {
            foreach (var point in line.Points)
            {
                var column = ColumnOf(point);
                var row = RowOf(point);
                var isInside = column >= 0 && row >= 0 && column < columns && row < rows;
                if (isInside) nearest.Offer(column, row, point);
            }
        }

        private int ColumnOf(GroundPoint place)
        {
            return (int)Math.Floor((place.East - west) / CellMetres);
        }

        private int RowOf(GroundPoint place)
        {
            return (int)Math.Floor((place.North - south) / CellMetres);
        }

        private GroundPoint CentreOf(int column, int row)
        {
            return new GroundPoint(west + (column + Halfway) * CellMetres, south + (row + Halfway) * CellMetres);
        }
    }
}
