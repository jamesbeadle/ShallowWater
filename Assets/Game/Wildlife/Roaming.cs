using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Roaming
    {
        private const double Arrived = 1;
        private const double Setting = 0;
        private const double HalfATurn = Math.PI;

        private readonly IReadOnlyList<WorldPoint> stations;
        private readonly Habit habit;
        private int at;
        private int towards;
        private double share = Setting;
        private double pauseSeconds;

        public Roaming(IReadOnlyList<WorldPoint> stations, Habit habit, Random random)
        {
            this.stations = stations;
            this.habit = habit;
            at = random.Next(stations.Count);
            towards = at;
            pauseSeconds = PauseFrom(random);
        }

        public bool IsMoving => pauseSeconds <= Setting && towards != at;
        public double Bearing => Heading.Bearing;

        public WorldPoint Position
        {
            get
            {
                var from = stations[at];
                var hop = habit.HopHeightMetres * Math.Sin(HalfATurn * share);
                return (Offset.Between(from, stations[towards]) * share + Offset.Up * hop).From(from);
            }
        }

        private GroundPoint Heading
        {
            get
            {
                var from = stations[at];
                var to = stations[towards];
                return new GroundPoint(to.East - from.East, to.North - from.North);
            }
        }

        public void Work(double seconds, Random random)
        {
            var isResting = pauseSeconds > Setting;
            if (isResting)
            {
                Rest(seconds, random);
                return;
            }
            var flight = Offset.Between(stations[at], stations[towards]).Length;
            share = Math.Min(Arrived, share + seconds * habit.SpeedMetresPerSecond / flight);
            if (share >= Arrived) Arrive(random);
        }

        public void Startle(Random random)
        {
            if (IsMoving) return;
            pauseSeconds = Setting;
            towards = Another(random);
        }

        private void Rest(double seconds, Random random)
        {
            pauseSeconds -= seconds;
            var isSettingOff = pauseSeconds <= Setting;
            if (isSettingOff) towards = Another(random);
        }

        private void Arrive(Random random)
        {
            at = towards;
            share = Setting;
            pauseSeconds = PauseFrom(random);
        }

        private int Another(Random random)
        {
            var other = random.Next(stations.Count - 1);
            return other >= at ? other + 1 : other;
        }

        private double PauseFrom(Random random)
        {
            return habit.ShortestPauseSeconds + random.NextDouble() * (habit.LongestPauseSeconds - habit.ShortestPauseSeconds);
        }
    }
}
