using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Triangulation
    {
        private const int CornersOfATriangle = 3;
        private const int NoEar = -1;

        public static List<int[]> OfClockwise(IReadOnlyList<GroundPoint> corners)
        {
            var remaining = Enumerable.Range(0, corners.Count).ToList();
            var triangles = new List<int[]>();
            while (remaining.Count >= CornersOfATriangle)
            {
                var ear = FirstEar(corners, remaining);
                var hasNoEar = ear == NoEar;
                if (hasNoEar) break;
                triangles.Add(EarCorners(remaining, ear));
                remaining.RemoveAt(ear);
            }
            return triangles;
        }

        private static int[] EarCorners(List<int> remaining, int ear)
        {
            var count = remaining.Count;
            return new[] { remaining[(ear + count - 1) % count], remaining[ear], remaining[(ear + 1) % count] };
        }

        private static int FirstEar(IReadOnlyList<GroundPoint> corners, List<int> remaining)
        {
            for (var ear = 0; ear < remaining.Count; ear++)
            {
                var candidate = EarCorners(remaining, ear);
                if (IsEar(corners, remaining, candidate)) return ear;
            }
            return NoEar;
        }

        private static bool IsEar(IReadOnlyList<GroundPoint> corners, List<int> remaining, int[] ear)
        {
            var before = corners[ear[0]];
            var tip = corners[ear[1]];
            var after = corners[ear[2]];
            var turnsAntiClockwise = Turn(before, tip, after) > 0;
            if (turnsAntiClockwise) return false;
            var others = remaining.Where(corner => !ear.Contains(corner));
            return !others.Any(corner => IsInside(corners[corner], before, tip, after));
        }

        private static bool IsInside(GroundPoint point, GroundPoint first, GroundPoint second, GroundPoint third)
        {
            return Turn(first, second, point) <= 0 && Turn(second, third, point) <= 0 && Turn(third, first, point) <= 0;
        }

        private static double Turn(GroundPoint from, GroundPoint through, GroundPoint to)
        {
            var incoming = through - from;
            var outgoing = to - through;
            return incoming.East * outgoing.North - incoming.North * outgoing.East;
        }
    }
}
