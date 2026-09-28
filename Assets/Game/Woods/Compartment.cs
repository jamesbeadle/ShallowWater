using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public sealed class Compartment
    {
        public Compartment(GroundPoint heart, bool isPlantation, double bearing, double scale)
        {
            Heart = heart;
            IsPlantation = isPlantation;
            Bearing = bearing;
            Scale = scale;
        }

        public GroundPoint Heart { get; }
        public bool IsPlantation { get; }
        public double Bearing { get; }
        public double Scale { get; }
    }
}
