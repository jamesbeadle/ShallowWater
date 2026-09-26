using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Unity.Map
{
    public static class FlatPoints
    {
        private const int CoordinatesPerPoint = 2;

        public static List<GroundPoint> Read(float[] coordinates)
        {
            var points = new List<GroundPoint>();
            for (var index = 0; index + 1 < coordinates.Length; index += CoordinatesPerPoint)
            {
                points.Add(new GroundPoint(coordinates[index], coordinates[index + 1]));
            }
            return points;
        }
    }
}
