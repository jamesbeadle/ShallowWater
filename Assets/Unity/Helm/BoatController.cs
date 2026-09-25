using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.Helm
{
    public sealed class BoatController : MonoBehaviour
    {
        private const float RadiansToDegrees = 180f / Mathf.PI;

        private BoatMotion motion;
        private Pound pound;

        public void Launch(BoatMotion boatMotion, Pound water)
        {
            motion = boatMotion;
            pound = water;
        }

        private void Update()
        {
            if (motion == null) return;
            motion.Advance(Time.deltaTime, HelmInput.Throttle(), HelmInput.Rudder());
            KeepInsideThePound();
            PlaceOnTheWater();
        }

        private void KeepInsideThePound()
        {
            var isAfloat = pound.Holds(motion.Position);
            if (isAfloat) return;
            motion.HoldAt(pound.Nearest(motion.Position));
        }

        private void PlaceOnTheWater()
        {
            var position = motion.Position;
            transform.position = new Vector3((float)position.Across, 0, (float)position.Along);
            var headingDegrees = (float)motion.HeadingRadians * RadiansToDegrees;
            transform.rotation = Quaternion.Euler(0, -headingDegrees, 0);
        }
    }
}
