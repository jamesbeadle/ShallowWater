using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewHead
    {
        public static SurfaceShapes Shape(Outfit outfit)
        {
            var head = new SurfaceShapes();
            head.Add(Surface.Skin, AskewSkull.Skin());
            head.Add(Surface.Stubble, AskewSkull.Stubble());
            head.Add(Surface.Hair, AskewSkull.Hair());
            AskewFace.Feature(head);
            AskewCap.Wear(head);
            return head;
        }
    }
}
