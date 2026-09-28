using System;
using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class TubeFrames
    {
        private const double NearlyUpright = 0.9;

        public static List<Offset[]> Along(IReadOnlyList<WorldPoint> path, bool isClosed)
        {
            var frames = new List<Offset[]>();
            var normal = FirstNormal(TangentAt(path, 0, isClosed));
            for (var index = 0; index < path.Count; index++)
            {
                var tangent = TangentAt(path, index, isClosed);
                normal = (normal - tangent * normal.Dot(tangent)).Normalised;
                frames.Add(new[] { normal, tangent.Cross(normal) });
            }
            return frames;
        }

        private static Offset FirstNormal(Offset tangent)
        {
            var isUpright = Math.Abs(tangent.Dot(Offset.Up)) > NearlyUpright;
            var reference = isUpright ? Offset.East : Offset.Up;
            return tangent.Cross(reference).Normalised;
        }

        private static Offset TangentAt(IReadOnlyList<WorldPoint> path, int index, bool isClosed)
        {
            var last = path.Count - 1;
            var before = isClosed ? (index + last) % path.Count : Math.Max(index - 1, 0);
            var after = isClosed ? (index + 1) % path.Count : Math.Min(index + 1, last);
            return Offset.Between(path[before], path[after]).Normalised;
        }
    }
}
