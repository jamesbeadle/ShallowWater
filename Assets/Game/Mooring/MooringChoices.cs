using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public static class MooringChoices
    {
        private static readonly MooringLine[] EndLines = { MooringLine.Bow, MooringLine.Stern };

        public static MooringLine? ToMakeFast(Mooring mooring, BoatMotion boat, GroundPoint feet)
        {
            if (mooring.CanMakeFast(MooringLine.Centre)) return MooringLine.Centre;
            var ends = EndLines.Where(line => mooring.CanMakeFast(line) && LineEnds.CanReach(line, boat, feet));
            return ends.Cast<MooringLine?>().FirstOrDefault();
        }

        public static MooringLine? ToLetGo(Mooring mooring, GroundPoint feet)
        {
            var ends = EndLines.Where(line => mooring.CanLetGo(line) && IsAtThePostOf(mooring, line, feet));
            var end = ends.Cast<MooringLine?>().FirstOrDefault();
            if (end.HasValue) return end;
            var canCastOff = mooring.CanLetGo(MooringLine.Centre) && IsAtThePostOf(mooring, MooringLine.Centre, feet);
            return canCastOff ? MooringLine.Centre : (MooringLine?)null;
        }

        private static bool IsAtThePostOf(Mooring mooring, MooringLine line, GroundPoint feet)
        {
            return MooringPosts.IsWithinReach(feet, mooring.LaidAt(line).Post);
        }
    }
}
