using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public readonly struct BandEdge
    {
        private const double Smooth = 0;

        public double Offset { get; }
        public double Height { get; }
        public double RoughnessMetres { get; }

        public BandEdge(double offset, double height) : this(offset, height, Smooth)
        {
        }

        public BandEdge(double offset, double height, double roughnessMetres)
        {
            Offset = offset;
            Height = height;
            RoughnessMetres = roughnessMetres;
        }

        public WorldPoint Beside(GroundPoint centre, GroundPoint spread)
        {
            var place = centre + spread * Offset;
            var height = Height + RoughnessMetres * Roughness.At(place);
            return new WorldPoint(place.East, height, place.North);
        }
    }
}
