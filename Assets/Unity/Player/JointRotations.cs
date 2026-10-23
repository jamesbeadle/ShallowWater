using ShallowWater.Game.People;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public static class JointRotations
    {
        public static Quaternion Of(FigurePart part, JointAngles angles)
        {
            var signed = JointAxes.Signed(part, angles);
            var twisted = Quaternion.AngleAxis((float)signed.TwistRadians * Mathf.Rad2Deg, Vector3.up);
            var swung = Quaternion.AngleAxis((float)signed.SwingRadians * Mathf.Rad2Deg, Vector3.right);
            var reached = Quaternion.AngleAxis((float)signed.ReachRadians * Mathf.Rad2Deg, Vector3.forward);
            return reached * swung * twisted;
        }
    }
}
