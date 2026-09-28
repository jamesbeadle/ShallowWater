using System;

namespace ShallowWater.Game.Boat
{
    public sealed class Engine
    {
        private const double StoppedShare = 0.02;
        private const int Ahead = 1;

        private int direction = Ahead;
        private double stoppedFor;

        public double Thrust { get; private set; }
        public double ThrustShare => Thrust / BoatHandling.FullThrust;

        public void Run(double seconds, ThrottleNotch notch)
        {
            var wanted = BoatHandling.DragAt(BoatHandling.SpeedAt[notch]);
            var isTheOtherWay = wanted * direction < 0;
            if (isTheOtherWay)
            {
                Reverse(seconds, wanted);
                return;
            }
            Thrust = Eased(Thrust, wanted, seconds);
        }

        private void Reverse(double seconds, double wanted)
        {
            Thrust = Eased(Thrust, 0, seconds);
            var hasStopped = Math.Abs(ThrustShare) < StoppedShare;
            stoppedFor = hasStopped ? stoppedFor + seconds : 0;
            if (stoppedFor < BoatHandling.ReversingPauseSeconds) return;
            direction = Math.Sign(wanted);
            stoppedFor = 0;
        }

        private static double Eased(double from, double to, double seconds)
        {
            var share = Math.Min(1, BoatHandling.EngineResponsePerSecond * seconds);
            return from + (to - from) * share;
        }
    }
}
