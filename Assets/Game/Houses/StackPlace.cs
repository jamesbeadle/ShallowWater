namespace ShallowWater.Game.Houses
{
    public readonly struct StackPlace
    {
        public StackPlace(double alongMetres, double halfAlongMetres, double halfAcrossMetres, int pots)
        {
            AlongMetres = alongMetres;
            HalfAlongMetres = halfAlongMetres;
            HalfAcrossMetres = halfAcrossMetres;
            Pots = pots;
        }

        public double AlongMetres { get; }
        public double HalfAlongMetres { get; }
        public double HalfAcrossMetres { get; }
        public int Pots { get; }
    }
}
