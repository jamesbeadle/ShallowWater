using System;

namespace ShallowWater.Game.Boat
{
    public sealed class Tiller
    {
        public double Rudder { get; private set; }

        public static double TurnedRadians(double rudder)
        {
            return -rudder * RudderForce.HardOverRadians;
        }

        public void Put(double seconds, double wanted)
        {
            var swing = BoatHandling.TillerSwingPerSecond * seconds;
            Rudder += Math.Clamp(wanted - Rudder, -swing, swing);
        }
    }
}
