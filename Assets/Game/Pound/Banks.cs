using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public static class Banks
    {
        private const double TouchingMetres = 1e-6;

        public static void Keep(BoatMotion boat, Pound pound)
        {
            var pushes = HullOutline.Points.Select(point => PushOn(boat, point, pound)).Where(IsTouching).ToList();
            if (!pushes.Any()) return;
            foreach (var push in pushes) boat.Strike(push.Offset, push.Away);
            boat.Shift(GroundPoint.MeanOf(pushes.Select(push => push.Correction).ToList()));
        }

        private static BankPush PushOn(BoatMotion boat, HullPoint point, Pound pound)
        {
            var place = boat.PointOf(point);
            var water = pound.WaterPositionAt(place);
            var kept = pound.KeptAfloat(water);
            var correction = pound.GroundPointAt(kept) - pound.GroundPointAt(water);
            return new BankPush(place - boat.Position, correction);
        }

        private static bool IsTouching(BankPush push)
        {
            return push.Depth > TouchingMetres;
        }
    }
}
