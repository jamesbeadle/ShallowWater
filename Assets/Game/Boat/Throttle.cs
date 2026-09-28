using System;

namespace ShallowWater.Game.Boat
{
    public sealed class Throttle
    {
        public ThrottleNotch Notch { get; private set; } = ThrottleNotch.Stop;

        public void Forward()
        {
            Notch = (ThrottleNotch)Math.Min((int)Notch + 1, (int)ThrottleNotch.FullAhead);
        }

        public void Back()
        {
            Notch = (ThrottleNotch)Math.Max((int)Notch - 1, (int)ThrottleNotch.FullAstern);
        }

        public void Stop()
        {
            Notch = ThrottleNotch.Stop;
        }
    }
}
