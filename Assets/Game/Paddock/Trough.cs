using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Paddock
{
    public static class Trough
    {
        private const double LengthMetres = 1.8;
        private const double WidthMetres = 0.65;
        private const double HeightMetres = 0.55;
        private const double WallMetres = 0.08;
        private const double WaterBelowTheRimMetres = 0.1;
        private const double Half = 0.5;

        public static void Stand(SurfaceShapes surfaces, GroundPoint centre, GroundPoint along)
        {
            var across = along.RightAngleClockwise;
            var sideOffset = (WidthMetres - WallMetres) * Half;
            var endOffset = (LengthMetres - WallMetres) * Half;
            Wall(surfaces, centre + across * sideOffset, along, LengthMetres * Half, WallMetres * Half);
            Wall(surfaces, centre - across * sideOffset, along, LengthMetres * Half, WallMetres * Half);
            Wall(surfaces, centre + along * endOffset, along, WallMetres * Half, WidthMetres * Half);
            Wall(surfaces, centre - along * endOffset, along, WallMetres * Half, WidthMetres * Half);
            var water = Footprints.Oblong(centre, along, LengthMetres * Half - WallMetres, WidthMetres * Half - WallMetres);
            surfaces.Add(Surface.Water, Extrusion.Roof(water, RimMetres - WaterBelowTheRimMetres));
        }

        public static GroundRing Footing(GroundPoint centre, GroundPoint along)
        {
            return Footprints.Oblong(centre, along, LengthMetres * Half, WidthMetres * Half);
        }

        public static WorldPoint RimPerch(GroundPoint centre, GroundPoint along)
        {
            var rim = centre + along.RightAngleClockwise * (WidthMetres * Half);
            return new WorldPoint(rim.East, RimMetres, rim.North);
        }

        private static double RimMetres => Heights.GroundMetres + HeightMetres;

        private static void Wall(SurfaceShapes surfaces, GroundPoint centre, GroundPoint along, double halfLengthMetres, double halfWidthMetres)
        {
            surfaces.AddSolid(Surface.Dressing, Footprints.Oblong(centre, along, halfLengthMetres, halfWidthMetres), Heights.GroundMetres, RimMetres);
        }
    }
}
