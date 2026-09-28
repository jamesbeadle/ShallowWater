using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class WoodFloor
    {
        public static Shape Under(Area area)
        {
            return Extrusion.Roof(area.Outline, Heights.WoodlandFloorMetres);
        }
    }
}
