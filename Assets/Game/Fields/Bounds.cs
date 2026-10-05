using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public readonly struct Bounds
    {
        public Bounds(double west, double east, double south, double north)
        {
            West = west;
            East = east;
            South = south;
            North = north;
        }

        public double West { get; }
        public double East { get; }
        public double South { get; }
        public double North { get; }
        public double WidthMetres => East - West;
        public double DepthMetres => North - South;

        public static Bounds Around(IReadOnlyList<GroundPoint> points, double marginMetres)
        {
            var west = points.Min(point => point.East) - marginMetres;
            var east = points.Max(point => point.East) + marginMetres;
            var south = points.Min(point => point.North) - marginMetres;
            var north = points.Max(point => point.North) + marginMetres;
            return new Bounds(west, east, south, north);
        }

        public bool IsOverlapping(Bounds other)
        {
            return West <= other.East && other.West <= East && South <= other.North && other.South <= North;
        }

        public bool IsAround(GroundPoint place)
        {
            return place.East >= West && place.East <= East && place.North >= South && place.North <= North;
        }
    }
}
