namespace ShallowWater.Game.Ground
{
    public readonly struct Span
    {
        private const double Half = 0.5;

        public Span(double from, double to)
        {
            From = from;
            To = to;
        }

        public static Span Around(double middle, double reach)
        {
            return new Span(middle - reach, middle + reach);
        }

        public double From { get; }
        public double To { get; }
        public double Length => To - From;
        public double Middle => From + Length * Half;

        public bool IsAround(double along)
        {
            return along >= From && along <= To;
        }
    }
}
