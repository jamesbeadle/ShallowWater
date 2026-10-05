using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public readonly struct WallFace
    {
        private const double ViewingDistanceMetres = 1;
        private const double Half = 0.5;

        public WallFace(GroundPoint from, GroundPoint to, Rise rise)
        {
            From = from;
            To = to;
            Rise = rise;
        }

        public GroundPoint From { get; }
        public GroundPoint To { get; }
        public Rise Rise { get; }
        public double LengthMetres => From.DistanceTo(To);
        public double HeightMetres => Rise.HeightMetres;
        public GroundPoint Lengthways => (To - From).Normalised;
        public GroundPoint Outward => Lengthways.RightAngleClockwise * -1;

        public WorldPoint Point(double along, double up)
        {
            var ground = From + Lengthways * along;
            return new WorldPoint(ground.East, Rise.FootMetres + up, ground.North);
        }

        public WorldPoint SeenFrom(double along, double up)
        {
            var ground = From + Lengthways * along + Outward * ViewingDistanceMetres;
            return new WorldPoint(ground.East, Rise.FootMetres + up, ground.North);
        }

        public WallFace Inset(double depthMetres)
        {
            var inward = Outward * -depthMetres;
            return new WallFace(From + inward, To + inward, Rise);
        }

        public WallFace Between(double along, double alongTo)
        {
            return new WallFace(From + Lengthways * along, From + Lengthways * alongTo, Rise);
        }

        public WallFace Raised(Rise rise)
        {
            return new WallFace(From, To, rise);
        }

        public GroundRing Slab(double alongFrom, double alongTo, double outFrom, double outTo)
        {
            var centre = From + Lengthways * ((alongFrom + alongTo) * Half) + Outward * ((outFrom + outTo) * Half);
            return Footprints.Oblong(centre, Lengthways, (alongTo - alongFrom) * Half, (outTo - outFrom) * Half);
        }

        public void AddRectangle(Shape shape, Opening area)
        {
            AddRectangle(shape, area, Fitting.None);
        }

        public void AddRectangle(Shape shape, Opening area, Fitting fitting)
        {
            var alongs = new[] { area.Left, area.Right, area.Right, area.Left };
            var ups = new[] { area.Bottom, area.Bottom, area.Top, area.Top };
            var corners = new WorldPoint[alongs.Length];
            var places = new SurfacePlace[alongs.Length];
            for (var corner = 0; corner < alongs.Length; corner++)
            {
                corners[corner] = Point(alongs[corner], ups[corner]);
                places[corner] = PlaceIn(area, alongs[corner], ups[corner]);
            }
            Facet.Add(shape, corners, places, SeenFrom(area.CentreAlong, area.MiddleHeight), fitting);
        }

        private static SurfacePlace PlaceIn(Opening area, double along, double up)
        {
            return new SurfacePlace(along - area.Left, up - area.Bottom);
        }
    }
}
