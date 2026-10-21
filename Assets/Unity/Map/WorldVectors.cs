using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class WorldVectors
    {
        public static Vector3 Of(WorldPoint point)
        {
            return new Vector3((float)point.East, (float)point.Height, (float)point.North);
        }

        public static WorldPoint PointOf(Vector3 vector)
        {
            return new WorldPoint(vector.x, vector.y, vector.z);
        }

        public static Offset OffsetOf(Vector3 vector)
        {
            return new Offset(vector.x, vector.y, vector.z);
        }

        public static Vector3 Of(Offset offset)
        {
            return new Vector3((float)offset.Eastward, (float)offset.Upward, (float)offset.Northward);
        }
    }
}
