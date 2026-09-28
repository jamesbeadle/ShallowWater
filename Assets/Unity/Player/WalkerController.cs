using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class WalkerController : MonoBehaviour
    {
        private const float FullTurnRadians = 2 * Mathf.PI;

        private readonly Walker walker = new Walker(new GroundPoint(0, 0), 0);
        private Land land;
        private AskewModel figure;
        private OrbitCamera view;
        private double phase;

        public GroundPoint Position => walker.Position;

        public void Ready(Land ground, AskewModel model, OrbitCamera camera)
        {
            land = ground;
            figure = model;
            view = camera;
        }

        public void SetOff(GroundPoint place, double bearing)
        {
            walker.StandAt(place, bearing);
            Place();
        }

        private void Update()
        {
            var seconds = Time.deltaTime;
            if (land == null || seconds <= 0) return;
            walker.Stride(seconds, WalkInput.Wish(view.YawDegrees), WalkInput.IsRunning(), land);
            var speed = walker.SpeedMetresPerSecond;
            phase = (phase + Gait.CyclesPerSecond(speed) * seconds * FullTurnRadians) % FullTurnRadians;
            figure.Pose(Gait.At(phase, speed));
            Place();
        }

        private void Place()
        {
            var position = walker.Position;
            transform.position = new Vector3((float)position.East, (float)land.HeightAt(position), (float)position.North);
            transform.rotation = Quaternion.Euler(0, (float)walker.Bearing * Mathf.Rad2Deg, 0);
        }
    }
}
