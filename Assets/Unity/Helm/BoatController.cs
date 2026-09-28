using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.Helm
{
    public sealed class BoatController : MonoBehaviour
    {
        private BoatMotion motion;
        private Pound pound;

        public float RudderShare { get; private set; }
        public float SpeedShare => motion == null ? 0 : (float)(motion.SpeedMetresPerSecond / BoatHandling.TopSpeedMetresPerSecond);

        public void Launch(BoatMotion boatMotion, Pound water)
        {
            motion = boatMotion;
            pound = water;
        }

        private void Update()
        {
            if (motion == null) return;
            var rudder = HelmInput.Rudder();
            RudderShare = (float)rudder;
            motion.Advance(Time.deltaTime, HelmInput.Throttle(), rudder);
            KeepInsideThePound();
            PlaceOnTheWater();
        }

        private void KeepInsideThePound()
        {
            var isAfloat = pound.IsOnTheWater(motion.Position);
            if (isAfloat) return;
            motion.HoldAt(pound.Nearest(motion.Position));
        }

        private void PlaceOnTheWater()
        {
            var position = motion.Position;
            var ground = pound.GroundPointAt(position);
            transform.position = new Vector3((float)ground.East, (float)Heights.WaterMetres, (float)ground.North);
            var bearing = pound.BearingAt(position.Along) + motion.HeadingRadians;
            transform.rotation = Quaternion.Euler(0, (float)bearing * Mathf.Rad2Deg, 0);
        }
    }
}
