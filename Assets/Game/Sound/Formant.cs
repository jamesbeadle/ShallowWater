namespace ShallowWater.Game.Sound
{
    public readonly struct Formant
    {
        public Formant(double hertz, double widthHertz, double share)
        {
            Hertz = hertz;
            WidthHertz = widthHertz;
            Share = share;
        }

        public double Hertz { get; }
        public double WidthHertz { get; }
        public double Share { get; }
    }
}
