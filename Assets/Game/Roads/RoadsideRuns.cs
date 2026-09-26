using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Roads
{
    public static class RoadsideRuns
    {
        private const double SampleSpacingMetres = 4;
        private const double ShortestPieceMetres = 10;
        private const double Half = 0.5;

        public static IReadOnlyList<RoadsidePiece> Along(Road road, double side, RoadSurroundings surroundings)
        {
            var samples = SamplesAlong(road, side, surroundings);
            var pieces = new List<RoadsidePiece>();
            var runStart = 0;
            for (var index = 1; index <= samples.Count; index++)
            {
                var isRunOver = index == samples.Count || samples[index] != samples[runStart];
                if (!isRunOver) continue;
                AddUnlessCleared(pieces, road, samples[runStart], runStart, index);
                runStart = index;
            }
            return pieces;
        }

        private static List<Roadside> SamplesAlong(Road road, double side, RoadSurroundings surroundings)
        {
            var count = (int)Math.Ceiling(road.LengthMetres / SampleSpacingMetres);
            var samples = new List<Roadside>(count);
            for (var sample = 0; sample < count; sample++)
            {
                var along = (sample + Half) * SampleSpacingMetres;
                samples.Add(surroundings.RoadsideAt(road, Math.Min(along, road.LengthMetres), side));
            }
            return samples;
        }

        private static void AddUnlessCleared(List<RoadsidePiece> pieces, Road road, Roadside roadside, int firstSample, int endSample)
        {
            var span = new Span(firstSample * SampleSpacingMetres, Math.Min(endSample * SampleSpacingMetres, road.LengthMetres));
            var isWorthDrawing = roadside != Roadside.Cleared && span.Length >= ShortestPieceMetres;
            if (!isWorthDrawing) return;
            pieces.Add(new RoadsidePiece(span, roadside));
        }
    }
}
