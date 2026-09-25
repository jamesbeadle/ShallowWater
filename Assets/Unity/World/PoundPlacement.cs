using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundPlacement
    {
        public static Quaternion AlongThePound(Pound pound, double along)
        {
            var bearing = (float)pound.BearingAt(along) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, bearing, 0);
        }

        public static Vector3 OnThePound(Pound pound, double along, double across, double height)
        {
            var ground = pound.GroundPointAt(new WaterPosition(along, across));
            return new Vector3((float)ground.East, (float)height, (float)ground.North);
        }
    }
}
