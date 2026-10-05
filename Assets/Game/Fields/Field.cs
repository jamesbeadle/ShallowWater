using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Fields
{
    public sealed class Field
    {
        public Field(ConvexOutline outline, Crop crop, GroundPoint workedAlong)
        {
            Outline = outline;
            Crop = crop;
            WorkedAlong = workedAlong;
        }

        public ConvexOutline Outline { get; }
        public Crop Crop { get; }
        public GroundPoint WorkedAlong { get; }
        public bool HasHeadland => Cropping.IsTilled(Crop);
    }
}
