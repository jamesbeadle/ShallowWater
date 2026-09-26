using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class PoundStretches
    {
        private const double ClosestCutsMetres = 0.5;

        private static readonly Reach Hopwas = Reach.From(Landmark.HopwasBridge, -200, 750);
        private static readonly Reach FazeleyToTamworth = Reach.Between(Landmark.FazeleyJunction, -350, Landmark.GlascoteLocks, 0);
        private static readonly Reach SouthOfHuddlesfordJunction = Reach.From(Landmark.HuddlesfordJunction, 60, 100);
        private static readonly Reach ByWhittington = Reach.From(Landmark.HuddlesfordJunction, 1140, 80);
        private static readonly Reach UnderHopwasHaysWood = Reach.From(Landmark.HopwasBridge, -950, 100);
        private static readonly Reach NorthOfHopwasBridge = Reach.From(Landmark.HopwasBridge, -150, 120);
        private static readonly Reach SouthOfHopwasBridge = Reach.From(Landmark.HopwasBridge, 30, 100);
        private static readonly Reach NorthOfFazeleyJunction = Reach.From(Landmark.FazeleyJunction, -230, 180);
        private static readonly Reach BelowGlascoteLocks = Reach.From(Landmark.GlascoteLocks, -140, 100);

        private static readonly Reach BirminghamAndFazeleyMouth = Reach.From(Landmark.FazeleyJunction, -2, 19);

        private static readonly Reach[] Villages = { Hopwas, FazeleyToTamworth };
        private static readonly Reach[] ArmMouths = { BirminghamAndFazeleyMouth };

        private static readonly Reach[] Moorings =
        {
            SouthOfHuddlesfordJunction, ByWhittington, UnderHopwasHaysWood,
            NorthOfHopwasBridge, SouthOfHopwasBridge, NorthOfFazeleyJunction, BelowGlascoteLocks,
        };

        public static IReadOnlyList<Stretch> Of(Pound pound)
        {
            var villages = SpansOf(Villages, pound);
            var moorings = SpansOf(Moorings, pound);
            var armMouths = SpansOf(ArmMouths, pound);
            var cuts = CutsAt(pound, villages.Concat(moorings).Concat(armMouths));
            var stretches = new List<Stretch>();
            for (var index = 1; index < cuts.Count; index++)
            {
                var span = new Span(cuts[index - 1], cuts[index]);
                stretches.Add(new Stretch(span, IsWithin(villages, span), IsWithin(moorings, span), IsWithin(armMouths, span)));
            }
            return stretches;
        }

        private static List<Span> SpansOf(IEnumerable<Reach> reaches, Pound pound)
        {
            return reaches.Select(reach => reach.SpanOn(pound)).ToList();
        }

        private static List<double> CutsAt(Pound pound, IEnumerable<Span> spans)
        {
            var start = pound.AlongOf(Landmark.HuddlesfordJunction);
            var end = pound.AlongOf(Landmark.GlascoteLocks);
            var ends = spans.SelectMany(span => new[] { span.From, span.To });
            var inside = ends.Where(along => along > start + ClosestCutsMetres && along < end - ClosestCutsMetres);
            var cuts = new List<double> { start };
            foreach (var along in inside.OrderBy(along => along)) AddUnlessTooClose(cuts, along);
            cuts.Add(end);
            return cuts;
        }

        private static void AddUnlessTooClose(List<double> cuts, double along)
        {
            var gap = along - cuts[cuts.Count - 1];
            var isTooClose = gap < ClosestCutsMetres;
            if (isTooClose) return;
            cuts.Add(along);
        }

        private static bool IsWithin(IEnumerable<Span> spans, Span stretch)
        {
            return spans.Any(span => span.IsAround(stretch.Middle));
        }
    }
}
