using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public readonly struct Band
    {
        private readonly BandEdge left;
        private readonly BandEdge right;

        public Band(Surface surface, BandEdge left, BandEdge right)
        {
            Surface = surface;
            this.left = left;
            this.right = right;
        }

        public Surface Surface { get; }

        public WorldPoint LeftBeside(GroundPoint centre, GroundPoint spread)
        {
            return left.Beside(centre, spread);
        }

        public WorldPoint RightBeside(GroundPoint centre, GroundPoint spread)
        {
            return right.Beside(centre, spread);
        }

        public static Band Flat(Surface surface, double width, double height)
        {
            var halfWidth = width / 2;
            return new Band(surface, new BandEdge(-halfWidth, height), new BandEdge(halfWidth, height));
        }
    }
}
