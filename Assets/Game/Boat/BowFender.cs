using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Boat
{
    public static class BowFender
    {
        private const double RadiusMetres = 0.07;
        private const int Sides = 8;
        private const double ProudOfTheStemMetres = 0.09;
        private const double LowestMetres = 0.05;
        private const int Points = 8;
        private const double BelowTheStemHeadMetres = 0.06;

        public static void Hang(SurfaceShapes surfaces)
        {
            var top = Hull.GunwaleAt(Hull.StemAlong) - BelowTheStemHeadMetres;
            var path = Enumerable.Range(0, Points).Select(point => OnTheStem(top - (top - LowestMetres) * point / (Points - 1))).ToList();
            surfaces.Add(Surface.TarredRope, Tube.Along(path, RadiusMetres, Sides));
        }

        private static WorldPoint OnTheStem(double height)
        {
            var stemHead = Hull.GunwaleAt(Hull.StemAlong);
            var upTheStem = (height - Hull.KeelMetres) / (stemHead - Hull.KeelMetres);
            var along = Hull.StemAlong + Hull.RakeAt(Hull.StemAlong) * upTheStem + ProudOfTheStemMetres;
            return new WorldPoint(BoatSides.Amidships, height, along);
        }
    }
}
