using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public readonly struct Reach
    {
        private readonly Landmark startLandmark;
        private readonly double startMetres;
        private readonly Landmark endLandmark;
        private readonly double endMetres;

        private Reach(Landmark startLandmark, double startMetres, Landmark endLandmark, double endMetres)
        {
            this.startLandmark = startLandmark;
            this.startMetres = startMetres;
            this.endLandmark = endLandmark;
            this.endMetres = endMetres;
        }

        public static Reach From(Landmark landmark, double startMetres, double lengthMetres)
        {
            return new Reach(landmark, startMetres, landmark, startMetres + lengthMetres);
        }

        public static Reach Between(Landmark startLandmark, double startMetres, Landmark endLandmark, double endMetres)
        {
            return new Reach(startLandmark, startMetres, endLandmark, endMetres);
        }

        public Span SpanOn(Pound pound)
        {
            var start = pound.AlongOf(startLandmark) + startMetres;
            var end = pound.AlongOf(endLandmark) + endMetres;
            return new Span(start, end);
        }
    }
}
