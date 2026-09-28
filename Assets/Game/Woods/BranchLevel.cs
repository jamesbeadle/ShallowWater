namespace ShallowWater.Game.Woods
{
    public sealed class BranchLevel
    {
        private const double UsualTipShare = 0.3;
        private const int SpiralWhorl = 1;

        public int Count { get; set; }
        public double FromShare { get; set; }
        public double AngleDegrees { get; set; }
        public double AngleSpreadDegrees { get; set; }
        public double LengthShare { get; set; }
        public double RadiusShare { get; set; }
        public double Rise { get; set; }
        public double Kink { get; set; }
        public int Segments { get; set; }
        public double TipShare { get; set; } = UsualTipShare;
        public int PerWhorl { get; set; } = SpiralWhorl;
    }
}
