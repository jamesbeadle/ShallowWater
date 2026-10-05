using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class CabinSection
    {
        public static WorldPoint Foot(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinFootHalfWidthMetres, SparrowForm.CabinFootMetres, along);
        }

        public static WorldPoint Top(double along, double side)
        {
            return new WorldPoint(side * SparrowForm.CabinTopHalfWidthMetres, SparrowForm.CabinSideTopMetres, along);
        }

        public static WorldPoint Crown(double along)
        {
            return new WorldPoint(BoatSides.Amidships, SparrowForm.CabinCrownMetres, along);
        }
    }
}
