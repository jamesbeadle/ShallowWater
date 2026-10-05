namespace ShallowWater.Game.Day
{
    public sealed class GameClock
    {
        public const double GameMinutesPerRealSecond = 1;
        public static readonly TimeOfDay MorningAtHopwas = TimeOfDay.At(8, 30);

        public TimeOfDay Now { get; private set; }

        public GameClock(TimeOfDay start)
        {
            Now = start;
        }

        public void Run(double realSeconds)
        {
            Now = Now.Later(realSeconds * GameMinutesPerRealSecond);
        }
    }
}
