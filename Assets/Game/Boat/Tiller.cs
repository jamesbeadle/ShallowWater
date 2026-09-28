using System;

namespace ShallowWater.Game.Boat
{
    public sealed class Tiller
    {
        public double Rudder { get; private set; }

        public void Put(double seconds, double wanted)
        {
            var swing = BoatHandling.TillerSwingPerSecond * seconds;
            Rudder += Math.Clamp(wanted - Rudder, -swing, swing);
        }
    }
}
