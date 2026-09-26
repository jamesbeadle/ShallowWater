namespace ShallowWater.Game.Pound
{
    public readonly struct Reach
    {
        private readonly Landmark landmark;
        private readonly double startMetres;
        private readonly double lengthMetres;

        private Reach(Landmark landmark, double startMetres, double lengthMetres)
        {
            this.landmark = landmark;
            this.startMetres = startMetres;
            this.lengthMetres = lengthMetres;
        }

        public static Reach From(Landmark landmark, double startMetres, double lengthMetres)
        {
            return new Reach(landmark, startMetres, lengthMetres);
        }

        public Span SpanOn(Pound pound)
        {
            var start = pound.AlongOf(landmark) + startMetres;
            return new Span(start, start + lengthMetres);
        }
    }
}
