using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class CrownShape
    {
        private const int Rings = 9;
        private const int Segments = 14;
        private const double UnitRadius = 0.5;
        private const double TopOfTheCrown = 1;
        private const double BottomOfTheCrown = -1;
        private const double FullTurnRadians = 2 * Math.PI;
        private const int PointsBeforeTheRings = 1;
        private const int FirstRing = 0;
        private const double Pole = 0;

        public static Shape Lumpy()
        {
            var crown = new Shape();
            var top = crown.Add(CrownLobes.Surface(Pole, TopOfTheCrown, Pole, UnitRadius));
            for (var ring = 0; ring < Rings; ring++) AddRing(crown, ring);
            var bottom = crown.Add(CrownLobes.Surface(Pole, BottomOfTheCrown, Pole, UnitRadius));
            for (var segment = 0; segment < Segments; segment++) Stitch(crown, segment, top, bottom);
            return crown;
        }

        private static void AddRing(Shape crown, int ring)
        {
            var fromTheTop = Math.PI * (ring + 1) / (Rings + 1);
            for (var segment = 0; segment < Segments; segment++)
            {
                var around = FullTurnRadians * segment / Segments;
                var spread = Math.Sin(fromTheTop);
                crown.Add(CrownLobes.Surface(spread * Math.Cos(around), Math.Cos(fromTheTop), spread * Math.Sin(around), UnitRadius));
            }
        }

        private static void Stitch(Shape crown, int segment, int top, int bottom)
        {
            var next = (segment + 1) % Segments;
            crown.AddTriangle(top, RingPoint(FirstRing, next), RingPoint(FirstRing, segment));
            for (var ring = 0; ring + 1 < Rings; ring++)
            {
                crown.AddQuad(RingPoint(ring, segment), RingPoint(ring + 1, segment), RingPoint(ring, next), RingPoint(ring + 1, next));
            }
            crown.AddTriangle(bottom, RingPoint(Rings - 1, segment), RingPoint(Rings - 1, next));
        }

        private static int RingPoint(int ring, int segment)
        {
            return PointsBeforeTheRings + ring * Segments + segment;
        }
    }
}
