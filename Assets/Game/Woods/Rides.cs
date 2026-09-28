using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public sealed class Rides
    {
        private const double HalfWidthMetres = 4.5;
        private const double SmallestWoodForARideMetres = 450;
        private const double SmallestWoodForACrossingMetres = 800;
        private readonly List<GroundPoint> headings = new List<GroundPoint>();
        private readonly GroundPoint centre;

        public Rides(GroundRing outline)
        {
            var corners = outline.Corners;
            centre = new GroundPoint(corners.Average(corner => corner.East), corners.Average(corner => corner.North));
            var width = outline.East - outline.West;
            var depth = outline.North - outline.South;
            var shortestSide = Math.Min(width, depth);
            var heading = GroundPoint.Facing(LongestBearing(corners));
            if (shortestSide > SmallestWoodForARideMetres) headings.Add(heading);
            if (shortestSide > SmallestWoodForACrossingMetres) headings.Add(heading.RightAngleClockwise);
        }

        public bool IsRideAt(GroundPoint place)
        {
            var fromTheCentre = place - centre;
            return headings.Any(heading => Math.Abs(fromTheCentre.Dot(heading.RightAngleClockwise)) < HalfWidthMetres);
        }

        private double LongestBearing(IReadOnlyList<GroundPoint> corners)
        {
            var spreads = corners.Select(corner => corner - centre).ToList();
            var eastward = spreads.Sum(spread => spread.East * spread.East);
            var northward = spreads.Sum(spread => spread.North * spread.North);
            var together = spreads.Sum(spread => spread.East * spread.North);
            var fromEast = Math.Atan2(2 * together, eastward - northward) / 2;
            return Math.PI / 2 - fromEast;
        }
    }
}
