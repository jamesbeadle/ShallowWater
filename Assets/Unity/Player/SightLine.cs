using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class SightLine
    {
        public static double ReachOf(Transform eye, double rangeMetres)
        {
            var isStopped = Physics.Raycast(eye.position, eye.forward, out var stop, (float)rangeMetres);
            return isStopped ? stop.distance : rangeMetres;
        }
    }
}
