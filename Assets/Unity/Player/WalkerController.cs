using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class WalkerController : MonoBehaviour
    {
        private readonly Walker walker = new Walker(new GroundPoint(0, 0), 0);
        private Land land;
        private FigureModel figure;
        private OrbitCamera view;
        private Tether tether = Tether.None;
        private double cycleShare;

        private const double ClosestToFaceMetres = 0.05;
        private const double Still = 0;
        private const double WholeCycle = 1;

        public GroundPoint Position => walker.Position;
        public double Bearing => walker.Bearing;
        public FigurePose Stride { get; private set; } = FigurePose.Upright;

        public void Ready(Land ground, FigureModel model, OrbitCamera camera)
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

        public void HeldBy(Tether line)
        {
            tether = line;
        }

        public void Face(GroundPoint place)
        {
            var towards = place - walker.Position;
            var isUnderfoot = towards.Length < ClosestToFaceMetres;
            if (isUnderfoot) return;
            SetOff(walker.Position, towards.Bearing);
        }

        public void Stand()
        {
            walker.StandAt(walker.Position, walker.Bearing);
            Stride = Gait.At(cycleShare, Still, Still, Time.time);
            figure.Pose(Stride);
            Place();
        }

        private void Update()
        {
            var seconds = Time.deltaTime;
            if (land == null || seconds <= 0) return;
            walker.Stride(seconds, WalkInput.Wish(view.YawDegrees), WalkInput.IsRunning(), land, tether);
            var speed = walker.SpeedMetresPerSecond;
            cycleShare = (cycleShare + Gait.CyclesPerSecond(speed) * seconds) % WholeCycle;
            Stride = Gait.At(cycleShare, speed, walker.TurnRadiansPerSecond, Time.time);
            figure.Pose(Stride);
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
