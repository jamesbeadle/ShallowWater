using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    [Serializable]
    public sealed class GroundRecord
    {
        public string image;
        public float west;
        public float east;
        public float south;
        public float north;
    }

    [Serializable]
    public sealed class LinesRecord
    {
        public LineRecord[] lines;
    }

    [Serializable]
    public sealed class LineRecord
    {
        public string kind;
        public float[] points;

        public GroundLine ToLine()
        {
            return new GroundLine(FlatPoints.Read(points));
        }
    }
}
