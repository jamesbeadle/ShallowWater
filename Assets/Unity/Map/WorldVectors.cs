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

        public static Vector3 Of(Offset offset)
        {
            return new Vector3((float)offset.Eastward, (float)offset.Upward, (float)offset.Northward);
        }
    }
}
