using System;
using System.Collections.Generic;
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
        private const int CoordinatesPerPoint = 2;

        public string kind;
        public float[] points;

        public GroundLine Line()
        {
            var places = new List<GroundPoint>();
            for (var index = 0; index + 1 < points.Length; index += CoordinatesPerPoint)
            {
                places.Add(new GroundPoint(points[index], points[index + 1]));
            }
            return new GroundLine(places);
        }
    }
}
