namespace ShallowWater.Game.Boat
{
    public static class SparrowForm
    {
        public const double CabinBackAlong = -9.5;
        public const double EngineRoomAlong = -6.7;
        public const double CabinFrontAlong = -4.2;
        public const double HatchFrontAlong = -8.65;
        public const double HatchHalfWidthMetres = 0.42;
        public const double HatchFloorMetres = 0.85;
        public const double DoorWidthMetres = HatchHalfWidthMetres;
        public const double DoorsFootMetres = HatchFloorMetres;
        public const double DoorsTopMetres = 1.76;
        public const double DoorStileMetres = 0.05;
        public const double CabinFootHalfWidthMetres = 0.97;
        public const double CabinTopHalfWidthMetres = 0.9;
        public const double CabinFootMetres = 0.3;
        public const double CabinSideTopMetres = 1.75;
        public const double CabinCrownMetres = 1.83;
        public const double RoofLipProudMetres = 0.025;
        public const double RoofLipHeightMetres = 0.045;
        public const double PanelInsetMetres = 0.14;
        public const double PanelFootMetres = 0.85;
        public const double PanelTopMetres = 1.6;
        public const double HoldFrontAlong = 5.7;
        public const double SideClothMetres = 0.35;
        public const double TopPlankMetres = 1.62;
        public const double TopPlankHalfWidthMetres = 0.11;
        public const double TopPlankThicknessMetres = 0.05;
        public const int ClothSpans = 11;
        public const double ClothSagMetres = 0.04;

        private const double Whole = 1;

        public static double RoofHeightAt(double across)
        {
            var fromTheEdge = Whole - System.Math.Abs(across) / CabinTopHalfWidthMetres;
            return CabinSideTopMetres + (CabinCrownMetres - CabinSideTopMetres) * fromTheEdge;
        }
    }
}
