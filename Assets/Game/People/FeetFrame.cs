using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FeetFrame
    {
        public static Offset Of(WorldPoint feet, double bearing, WorldPoint target)
        {
            var world = Offset.Between(feet, target);
            var right = world.Eastward * Math.Cos(bearing) - world.Northward * Math.Sin(bearing);
            var forward = world.Eastward * Math.Sin(bearing) + world.Northward * Math.Cos(bearing);
            return new Offset(right, world.Upward, forward);
        }
    }
}
