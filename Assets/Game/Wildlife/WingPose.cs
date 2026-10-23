using System;

namespace ShallowWater.Game.Wildlife
{
    public static class WingPose
    {
        public const double FoldedBackRadians = 1.4;
        public const double FoldedDroopRadians = 0.6;
        private const double BeatsPerSecond = 3.6;
        private const double StrokeRadians = 0.85;
        private const double HeldAboveTheBackRadians = 0.2;
        private const double FullTurnRadians = 2 * Math.PI;

        public static double RaisedAt(double seconds)
        {
            return HeldAboveTheBackRadians + StrokeRadians * Math.Sin(FullTurnRadians * BeatsPerSecond * seconds);
        }
    }
}
