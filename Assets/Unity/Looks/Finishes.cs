using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class Finishes
    {
        private static readonly Dictionary<Surface, Material> Made = new Dictionary<Surface, Material>();

        public static Material For(Surface surface)
        {
            var isMade = Made.TryGetValue(surface, out var finish);
            var isStillAlive = isMade && finish != null;
            if (isStillAlive) return finish;
            finish = SurfaceLooks.Made(surface);
            Made[surface] = finish;
            return finish;
        }
    }
}
