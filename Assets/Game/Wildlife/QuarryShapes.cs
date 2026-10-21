using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public static class QuarryShapes
    {
        public static SurfaceShapes Of(QuarryKind kind)
        {
            return QuarryHabits.IsACorvid(kind) ? CorvidShapes.Of(kind) : RatShapes.Rat();
        }
    }
}
