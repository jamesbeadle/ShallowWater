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
            Stovepipes.Build(surfaces);
            RoofStowage.Build(surfaces);
            HoldShapes.Build(surfaces);
            ForeEnd.Build(surfaces);
            SternGear.Build(surfaces);
            return surfaces;
        }
    }
}
