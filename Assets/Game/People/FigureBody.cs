using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureBody
    {
        public static SurfaceShapes Torso(Outfit outfit)
        {
            var torso = new SurfaceShapes();
            AskewCoat.Dress(torso, outfit.Coat);
            if (outfit.HasAScarf) AskewScarf.Wrap(torso);
            return torso;
        }

        public static SurfaceShapes HeadAndCap(Outfit outfit)
        {
            var head = new SurfaceShapes();
            AskewHead.Shape(head);
            AskewCap.Wear(head);
            return head;
        }
    }
}
