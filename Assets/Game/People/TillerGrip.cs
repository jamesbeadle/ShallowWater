using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class TillerGrip
    {
        private const double Level = 0;
        private const double HandleStart = 0;
        private const double HandleFinish = 1;
        private static readonly WorldPoint Stock = new WorldPoint(Level, Level, SternGear.StockAlong);

        public static Offset FromTheFeet(double tillerTurnedRadians, Offset shoulderFromTheFeet)
        {
            var neck = HandleFromTheFeet(SternGear.HandleNeck, tillerTurnedRadians);
            var end = HandleFromTheFeet(SternGear.HandleEnd, tillerTurnedRadians);
            var handle = end - neck;
            var alongTheHandle = (shoulderFromTheFeet - neck).Dot(handle) / handle.Dot(handle);
            return neck + handle * Math.Clamp(alongTheHandle, HandleStart, HandleFinish);
        }

        private static Offset HandleFromTheFeet(WorldPoint onTheHandle, double tillerTurnedRadians)
        {
            var fromTheStock = new Offset(onTheHandle.East, onTheHandle.Height, onTheHandle.North).TurnedAbout(Offset.Up, tillerTurnedRadians);
            return Offset.Between(HelmStance.Feet, fromTheStock.From(Stock));
        }
    }
}
