namespace ShallowWater.Game.Boat
{
    public readonly struct BoatWay
    {
        public BoatWay(double ahead, double aside, double swing)
        {
            Ahead = ahead;
            Aside = aside;
            Swing = swing;
        }

        public double Ahead { get; }
        public double Aside { get; }
        public double Swing { get; }

        public double AsideAt(double aheadMetres)
        {
            return Aside + aheadMetres * Swing;
        }
    }
}
