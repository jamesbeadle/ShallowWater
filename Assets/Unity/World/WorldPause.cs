using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class WorldPause
    {
        private const float Stopped = 0f;
        private const float Running = 1f;

        public static void Begin()
        {
            Time.timeScale = Stopped;
            AudioListener.pause = true;
        }

        public static void End()
        {
            Time.timeScale = Running;
            AudioListener.pause = false;
        }
    }
}
