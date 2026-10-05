using System;

namespace ShallowWater.Game.Boat
{
    public static class HullCrossFlow
    {
        private const int Strips = 10;
        private const double StripMetres = BoatSize.LengthMetres / Strips;
        private const double FirstStripAheadMetres = StripMetres / 2 - BoatSize.HalfLengthMetres;
        private const double CrossDragCoefficient = 2.0;
        private const double CreepNewtonsPerMetrePerSecond = 60;
        private const double LiftCoefficient = 0.22;
        private const double LiftAheadMetres = BoatSize.LengthMetres / 4;
        private const double Half = 0.5;
        private const double SideSquareMetres = BoatSize.LengthMetres * BoatSize.DraughtMetres;

        public static BoatForces On(BoatWay way)
        {
            var forces = Lift(way);
            for (var strip = 0; strip < Strips; strip++) forces += OnStrip(way, FirstStripAheadMetres + strip * StripMetres);
            return forces;
        }

        private static BoatForces OnStrip(BoatWay way, double aheadMetres)
        {
            var aside = way.AsideAt(aheadMetres);
            var drag = Half * FreshWater.KilogramsPerCubicMetre * BoatSize.DraughtMetres * CrossDragCoefficient * aside * Math.Abs(aside);
            var creep = CreepNewtonsPerMetrePerSecond * aside;
            return BoatForces.Aside(aheadMetres, -(drag + creep) * StripMetres);
        }

        private static BoatForces Lift(BoatWay way)
        {
            var leadingEnd = Math.Sign(way.Ahead) * LiftAheadMetres;
            var lift = Half * FreshWater.KilogramsPerCubicMetre * SideSquareMetres * LiftCoefficient * Math.Abs(way.Ahead) * way.Aside;
            return BoatForces.Aside(leadingEnd, -lift);
        }
    }
}
