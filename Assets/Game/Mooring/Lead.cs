using System;
using System.Collections.Generic;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public sealed class Lead
    {
        private const double SameSurfaceMetres = 0.03;
        private const int Halvings = 3;
        private const double Half = 0.5;
        private const double NoRun = 1e-9;

        private readonly SurfaceBeneath beneath;
        private readonly GroundPoint from;
        private readonly GroundPoint direction;
        private readonly LeadStations stations;
        private readonly IReadOnlyList<LeadPoint> points;

        public Lead(GroundPoint from, GroundPoint to, SurfaceBeneath beneath)
        {
            this.beneath = beneath;
            this.from = from;
            var run = to - from;
            LengthMetres = run.Length;
            direction = LengthMetres > NoRun ? run.Normalised : BoatSides.Ahead;
            stations = new LeadStations(from, direction, LengthMetres, beneath);
            var samples = new LeadSamples(LengthMetres, PointAt);
            points = samples.Points;
        }

        public double LengthMetres { get; }
        public int Count => points.Count;
        public LeadPoint this[int index] => points[index];
        public GroundPoint Aside => direction.RightAngleClockwise;

        public WorldPoint At(int index, double height)
        {
            var place = points[index].Place;
            return new WorldPoint(place.East, height, place.North);
        }

        public WorldPoint RestingBeside(int index, double offsetMetres)
        {
            var point = points[index];
            var offset = offsetMetres;
            for (var halving = 0; halving < Halvings; halving++)
            {
                var step = Aside * offset;
                var place = point.Place + step;
                var water = beneath.WaterPositionBeside(point.Water, step);
                var resting = beneath.RestingHeightAt(place, water);
                var isOnTheSameSurface = Math.Abs(resting - point.RestingHeight) < SameSurfaceMetres;
                if (isOnTheSameSurface) return new WorldPoint(place.East, resting, place.North);
                offset *= Half;
            }
            return At(index, point.RestingHeight);
        }

        private LeadPoint PointAt(double along)
        {
            var place = from + direction * along;
            var water = stations.At(along);
            return new LeadPoint(along, place, water, beneath.RestingHeightAt(place, water));
        }
    }
}
