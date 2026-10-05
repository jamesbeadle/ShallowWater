using ShallowWater.Game.Day;
using UnityEngine;

namespace ShallowWater.Unity.Sky
{
    public static class SkyAngles
    {
        private const float HalfTurnDegrees = 180f;

        public static Quaternion ShiningFrom(SkyPosition body)
        {
            var downwards = (float)body.HeightDegrees;
            var awayFromTheBody = (float)body.BearingDegrees + HalfTurnDegrees;
            return Quaternion.Euler(downwards, awayFromTheBody, 0);
        }
    }
}
