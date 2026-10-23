using System;
using System.Collections.Generic;
using ShallowWater.Game.Boat;

namespace ShallowWater.Game.Ground
{
    public sealed class Centreline
    {
        private readonly double[] distances;
        private readonly TurningBearings bearings;

        public Centreline(GroundLine line)
        {
            Line = line;
            distances = line.DistancesAlong();
            bearings = new TurningBearings(line);
        }

        public GroundLine Line { get; }
        public IReadOnlyList<double> Distances => distances;
        public double LengthMetres => distances[distances.Length - 1];
        public Span Whole => new Span(0, LengthMetres);

        public double BearingAt(double along)
        {
            var segment = SegmentAt(along);
            var length = SegmentLength(segment);
            var intoSegment = Math.Clamp(along - distances[segment], 0, length);
            return bearings.At(segment, intoSegment, length);
        }

        public GroundPoint Offset(double along, double across)
        {
            var segment = SegmentAt(along);
            var fraction = FractionThrough(segment, along);
            var start = Line[segment];
            var onTheLine = start + (Line[segment + 1] - start) * fraction;
            var right = GroundPoint.Facing(BearingAt(along)).RightAngleClockwise;
            return onTheLine + right * across;
        }

        public WaterPosition NearestWithin(GroundPoint point, Span span)
        {
            var nearestSegment = SegmentAt(span.From);
            var lastSegment = SegmentAt(span.To);
            var nearestDistance = DistanceFrom(nearestSegment, point);
            for (var segment = nearestSegment + 1; segment <= lastSegment; segment++)
            {
                var distance = DistanceFrom(segment, point);
                if (distance >= nearestDistance) continue;
                nearestSegment = segment;
                nearestDistance = distance;
            }
            return ProjectedOnto(nearestSegment, point);
        }

        private int SegmentAt(double along)
        {
            var clamped = Math.Clamp(along, 0, LengthMetres);
            var found = Array.BinarySearch(distances, clamped);
            var segment = found >= 0 ? found : ~found - 1;
            return Math.Clamp(segment, 0, Line.SegmentCount - 1);
        }

        private double SegmentLength(int segment)
        {
            return distances[segment + 1] - distances[segment];
        }

        private double FractionThrough(int segment, double along)
        {
            var intoSegment = along - distances[segment];
            return Math.Clamp(intoSegment / SegmentLength(segment), 0, 1);
        }

        private GroundPoint FootOn(int segment, GroundPoint point)
        {
            var start = Line[segment];
            var direction = Line.Direction(segment);
            var along = Math.Clamp((point - start).Dot(direction), 0, SegmentLength(segment));
            return start + direction * along;
        }

        private double DistanceFrom(int segment, GroundPoint point)
        {
            return point.DistanceTo(FootOn(segment, point));
        }

        private WaterPosition ProjectedOnto(int segment, GroundPoint point)
        {
            var foot = FootOn(segment, point);
            var along = distances[segment] + foot.DistanceTo(Line[segment]);
            var right = Line.Direction(segment).RightAngleClockwise;
            return new WaterPosition(along, (point - foot).Dot(right));
        }
    }
}
