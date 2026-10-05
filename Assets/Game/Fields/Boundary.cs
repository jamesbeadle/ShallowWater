namespace ShallowWater.Game.Fields
{
    public readonly struct Boundary
    {
        private const int NoNeighbour = -1;

        public static readonly Boundary Open = new Boundary(BoundaryKind.Open, false, NoNeighbour);
        public static readonly Boundary FieldHedge = new Boundary(BoundaryKind.FieldHedge, false, NoNeighbour);

        public Boundary(BoundaryKind kind, bool hasFootpath, int neighbour)
        {
            Kind = kind;
            HasFootpath = hasFootpath;
            Neighbour = neighbour;
        }

        public BoundaryKind Kind { get; }
        public bool HasFootpath { get; }
        public int Neighbour { get; }
        public bool HasHedge => Kind != BoundaryKind.Open;

        public static Boundary FarmHedge(bool hasFootpath, int neighbour)
        {
            return new Boundary(BoundaryKind.FarmHedge, hasFootpath, neighbour);
        }
    }
}
