using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class Farm
    {
        public Farm(int index, ConvexOutline outline, double grainBearing)
        {
            Index = index;
            Outline = outline;
            GrainBearing = grainBearing;
        }

        public int Index { get; }
        public ConvexOutline Outline { get; }
        public double GrainBearing { get; }
        public GroundPoint Grain => GroundPoint.Facing(GrainBearing);
        public bool IsDrawable => Outline.IsDrawable;
    }
}
