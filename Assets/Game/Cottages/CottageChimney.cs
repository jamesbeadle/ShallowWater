using System;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Cottages
{
    public static class CottageChimney
    {
        private const double OnTheRidge = 0;
        private const long EveryOther = 2;
        private const double FrontEnd = 1;
        private const double BackEnd = -1;

        public static void Build(SurfaceShapes surfaces, Cottage cottage)
        {
            var along = EndOf(cottage) * (cottage.HalfLengthMetres - CottageForm.ChimneyInFromTheGableMetres);
            var centre = cottage.At(along, OnTheRidge);
            var stack = Footprints.Oblong(centre, cottage.Crossways, CottageForm.ChimneyHalfSpanMetres, CottageForm.ChimneyHalfDepthMetres);
            var foot = cottage.RidgeMetres - CottageForm.ChimneyFootBelowTheRidgeMetres;
            var top = cottage.RidgeMetres + CottageForm.ChimneyAboveTheRidgeMetres;
            surfaces.AddSolid(Surface.Brickwork, stack, foot, top);
            Pot(surfaces, cottage.At(along, -CottageForm.PotSpacingMetres), top);
            Pot(surfaces, cottage.At(along, CottageForm.PotSpacingMetres), top);
        }

        private static double EndOf(Cottage cottage)
        {
            var centre = cottage.Centre;
            var place = (long)Math.Floor(centre.East + centre.North);
            var isEven = place % EveryOther == 0;
            return isEven ? FrontEnd : BackEnd;
        }

        private static void Pot(SurfaceShapes surfaces, GroundPoint centre, double foot)
        {
            var pot = Footprints.Round(centre, CottageForm.PotRadiusMetres, CottageForm.PotSides);
            surfaces.AddSolid(Surface.ChimneyPot, pot, foot, foot + CottageForm.PotHeightMetres);
        }
    }
}
