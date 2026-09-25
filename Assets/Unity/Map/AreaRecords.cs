using System;
using System.Linq;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    [Serializable]
    public sealed class AreasRecord
    {
        public AreaRecord[] areas;
    }

    [Serializable]
    public sealed class AreaRecord
    {
        public float[] outline;
        public RingRecord[] holes;

        public Area ToArea()
        {
            var ring = new GroundRing(FlatPoints.Read(outline));
            return new Area(ring, holes.Select(hole => hole.ToRing()));
        }
    }

    [Serializable]
    public sealed class RingRecord
    {
        public float[] points;

        public GroundRing ToRing()
        {
            return new GroundRing(FlatPoints.Read(points));
        }
    }
}
