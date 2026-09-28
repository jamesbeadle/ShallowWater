using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public readonly struct LeafClump
    {
        public LeafClump(WorldPoint centre, double sizeMetres, Offset outward, double depth, double seed)
        {
            Centre = centre;
            SizeMetres = sizeMetres;
            Outward = outward;
            Depth = depth;
            Seed = seed;
        }

        public WorldPoint Centre { get; }
        public double SizeMetres { get; }
        public Offset Outward { get; }
        public double Depth { get; }
        public double Seed { get; }
    }
}
