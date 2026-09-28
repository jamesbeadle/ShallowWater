using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class SparrowShapes
    {
        public static SurfaceShapes MooredAtTheOrigin()
        {
            var surfaces = new SurfaceShapes();
            HullShapes.Build(surfaces);
            CabinShapes.Build(surfaces);
            HoldShapes.Build(surfaces);
            Fittings.Build(surfaces);
            return surfaces;
        }
    }
}
