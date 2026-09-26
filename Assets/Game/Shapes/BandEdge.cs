using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public readonly struct BandEdge
    {
        public double Offset { get; }
        public double Height { get; }

        public BandEdge(double offset, double height)
        {
            Offset = offset;
            Height = height;
        }

        public WorldPoint Beside(GroundPoint centre, GroundPoint spread)
        {
            var place = centre + spread * Offset;
            return new WorldPoint(place.East, Height, place.North);
        }
    }
}
