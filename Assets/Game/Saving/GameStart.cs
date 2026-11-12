using ShallowWater.Game.Boat;
using ShallowWater.Game.Day;
using ShallowWater.Game.Pound;
using CanalPound = ShallowWater.Game.Pound.Pound;

namespace ShallowWater.Game.Saving
{
    public sealed class GameStart
    {
        private const double MooredOffTheBankMetres = 0.3;

        private readonly SavedGame saved;

        private GameStart(SavedGame saved)
        {
            this.saved = saved;
        }

        public static GameStart Fresh => new GameStart(null);
        public bool IsFresh => saved == null;
        public TimeOfDay Time => IsFresh ? GameClock.MorningAtHopwas : saved.Time;

        public static GameStart From(SavedGame saved)
        {
            return new GameStart(saved);
        }

        public BoatMotion BoatOn(CanalPound pound)
        {
            if (IsFresh) return MooredAtHopwas(pound);
            return new BoatMotion(saved.BoatPosition, saved.BoatBearing);
        }

        private static BoatMotion MooredAtHopwas(CanalPound pound)
        {
            var bridge = pound.AlongOf(Landmark.HopwasBridge);
            var againstTheTowpath = PoundLimits.TowpathSide * (pound.HalfWidth - BoatSize.BeamMetres / 2 - MooredOffTheBankMetres);
            var mooring = pound.GroundPointAt(new WaterPosition(bridge, againstTheTowpath));
            return new BoatMotion(mooring, pound.BearingAt(bridge));
        }
    }
}
