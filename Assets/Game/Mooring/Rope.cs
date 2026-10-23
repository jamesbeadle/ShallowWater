using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class Rope
    {
        public const double RadiusMetres = 0.012;
        public const double DiameterMetres = 2 * RadiusMetres;
        private const int Sides = 8;
        private const double SunkMetres = RadiusMetres / 3;

        public static double Afloat => Heights.WaterMetres - SunkMetres;

        public static double RestingOn(double surfaceMetres)
        {
            return surfaceMetres + RadiusMetres;
        }

        public static Shape Laid(IReadOnlyList<WorldPoint> path)
        {
            return Tube.Along(path, RadiusMetres, Sides);
        }
    }
}
