using System;

namespace ShallowWater.Game.Boat
{
    public static class SparrowTop
    {
        public static bool IsOver(HullPoint point)
        {
            var isWithinTheLength = Math.Abs(point.Ahead) <= BoatSize.HalfLengthMetres;
            return isWithinTheLength && Math.Abs(point.ToStarboard) <= Hull.HalfBeamAt(point.Ahead);
        }

        public static double HeightAt(HullPoint point)
        {
            var along = point.Ahead;
            var across = Math.Abs(point.ToStarboard);
            var isUnderTheCloths = along > SparrowForm.CabinFrontAlong && along < SparrowForm.HoldFrontAlong;
            if (isUnderTheCloths) return Sheeting.HeightAt(along, across);
            var isOnTheGunwale = across > Hull.HalfBeamAt(along) - HullShapes.PlateMetres;
            if (isOnTheGunwale) return Hull.GunwaleAt(along);
            var isBesideTheCabin = along >= SparrowForm.CabinBackAlong && along <= SparrowForm.CabinFrontAlong;
            var isOnTheRoof = isBesideTheCabin && across <= SparrowForm.CabinTopHalfWidthMetres;
            if (isOnTheRoof) return SparrowForm.RoofHeightAt(across);
            return Hull.DeckAt(along);
        }
    }
}
