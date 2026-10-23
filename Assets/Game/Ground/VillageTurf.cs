using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Ground
{
    public static class VillageTurf
    {
        public static Shape Under(Area village)
        {
            return Extrusion.Roof(village.Outline, Heights.VillageTurfMetres);
        }
    }
}
