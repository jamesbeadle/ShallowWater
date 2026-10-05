using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class FieldDivision
    {
        private const double SmallestFieldSquareMetres = 30000;
        private const double FieldSizeRangeSquareMetres = 55000;
        private const double NarrowestFieldMetres = 85;
        private const double CutFrom = 0.38;
        private const double CutSpread = 0.24;
        private const double GrainWanderRadians = 0.12;
        private const double Middle = 0.5;
        private const int EndsOfAChord = 2;

        private readonly Farm farm;
        private readonly Random random;
        private readonly List<ConvexOutline> fields = new List<ConvexOutline>();
        private readonly List<Chord> chords = new List<Chord>();

        private FieldDivision(Farm farm, Random random)
        {
            this.farm = farm;
            this.random = random;
        }

        public IReadOnlyList<ConvexOutline> Fields => fields;
        public IReadOnlyList<Chord> Chords => chords;

        public static FieldDivision Of(Farm farm, Random random)
        {
            var division = new FieldDivision(farm, random);
            division.Divide(farm.Outline);
            return division;
        }

        private void Divide(ConvexOutline outline)
        {
            var targetSquareMetres = SmallestFieldSquareMetres + random.NextDouble() * FieldSizeRangeSquareMetres;
            var isSmallEnough = outline.Area <= targetSquareMetres;
            var line = CutAcross(outline);
            var right = OutlineCut.KeepRight(outline, line, Boundary.FieldHedge);
            var left = OutlineCut.KeepRight(outline, line.Reversed, Boundary.FieldHedge);
            var crossings = OutlineCut.Crossings(outline, line);
            var canBeCut = IsWideEnough(right, line) && IsWideEnough(left, line) && crossings.Count == EndsOfAChord;
            if (isSmallEnough || !canBeCut)
            {
                fields.Add(outline);
                return;
            }
            chords.Add(new Chord(crossings[0], crossings[1]));
            Divide(right);
            Divide(left);
        }

        private CuttingLine CutAcross(ConvexOutline outline)
        {
            var grain = GroundPoint.Facing(farm.GrainBearing + (random.NextDouble() - Middle) * GrainWanderRadians);
            var longer = outline.LongerOf(grain);
            var reaches = outline.Corners.Select(corner => corner.Dot(longer)).ToList();
            var share = CutFrom + random.NextDouble() * CutSpread;
            var cutAt = reaches.Min() + (reaches.Max() - reaches.Min()) * share;
            var centroid = outline.Centroid();
            var through = centroid + longer * (cutAt - centroid.Dot(longer));
            return new CuttingLine(through, longer.RightAngleClockwise);
        }

        private static bool IsWideEnough(ConvexOutline part, CuttingLine line)
        {
            return part.IsDrawable && part.WidthAcross(line.Across) >= NarrowestFieldMetres;
        }
    }
}
