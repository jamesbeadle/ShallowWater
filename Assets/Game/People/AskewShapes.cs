using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewShapes
    {
        public static SurfaceShapes Of(FigurePart part)
        {
            return FigureShapes.Of(part, Outfit.Askew);
        }
    }
}
