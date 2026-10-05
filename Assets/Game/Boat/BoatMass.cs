namespace ShallowWater.Game.Boat
{
    public static class BoatMass
    {
        private const double DisplacementKilograms = 18000;
        private const double WaterDraggedAheadShare = 0.08;
        private const double WaterDraggedAsideShare = 1.2;
        private const double WaterDraggedRoundShare = 1.0;
        private const double TwelfthOfTheLengthSquared = BoatSize.LengthMetres * BoatSize.LengthMetres / 12;

        public const double AheadKilograms = DisplacementKilograms * (1 + WaterDraggedAheadShare);
        public const double AsideKilograms = DisplacementKilograms * (1 + WaterDraggedAsideShare);
        public const double SwingInertia = DisplacementKilograms * (1 + WaterDraggedRoundShare) * TwelfthOfTheLengthSquared;
        public const double SwingRadiusSquared = SwingInertia / AsideKilograms;
    }
}
