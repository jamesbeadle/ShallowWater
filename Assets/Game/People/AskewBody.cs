using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewBody
    {
        public static SurfaceShapes Torso()
        {
            var torso = new SurfaceShapes();
            AskewCoat.Dress(torso);
            AskewScarf.Wrap(torso);
            return torso;
        }

        public static SurfaceShapes HeadAndCap()
        {
            var head = new SurfaceShapes();
            AskewHead.Shape(head);
            AskewCap.Wear(head);
            return head;
        }
    }
}
