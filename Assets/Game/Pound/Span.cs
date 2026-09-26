namespace ShallowWater.Game.Pound
{
    public readonly struct Span
    {
        private const double Half = 0.5;

        public Span(double from, double to)
        {
            From = from;
            To = to;
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
