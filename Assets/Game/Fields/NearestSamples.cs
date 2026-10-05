using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class NearestSamples
    {
        private const int Before = -1;
        private const int After = 1;

        private readonly int columns;
        private readonly int rows;
        private readonly Func<int, int, GroundPoint> centreOf;
        private readonly GroundPoint?[] samples;

        public NearestSamples(int columns, int rows, Func<int, int, GroundPoint> centreOf)
        {
            this.columns = columns;
            this.rows = rows;
            this.centreOf = centreOf;
            samples = new GroundPoint?[columns * rows];
        }

        public double DistanceAt(int column, int row)
        {
            var sample = samples[row * columns + column];
            if (sample == null) return double.MaxValue;
            return centreOf(column, row).DistanceTo(sample.Value);
        }

        public void Offer(int column, int row, GroundPoint sample)
        {
            var isNearer = centreOf(column, row).DistanceTo(sample) < DistanceAt(column, row);
            if (isNearer) samples[row * columns + column] = sample;
        }

        public void Spread()
        {
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++) TakeFrom(column, row, Before);
            }
            for (var row = rows - 1; row >= 0; row--)
            {
                for (var column = columns - 1; column >= 0; column--) TakeFrom(column, row, After);
            }
        }

        private void TakeFrom(int column, int row, int step)
        {
            OfferFrom(column, row, column + step, row);
            OfferFrom(column, row, column, row + step);
            OfferFrom(column, row, column + step, row + step);
            OfferFrom(column, row, column - step, row + step);
        }

        private void OfferFrom(int column, int row, int fromColumn, int fromRow)
        {
            var isOutside = fromColumn < 0 || fromRow < 0 || fromColumn >= columns || fromRow >= rows;
            if (isOutside) return;
            var sample = samples[fromRow * columns + fromColumn];
            if (sample != null) Offer(column, row, sample.Value);
        }
    }
}
