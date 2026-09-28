using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Cottages
{
    public static class CottageRoof
    {
        private static readonly double[] Sides = { -1, 1 };

        public static void Lay(SurfaceShapes surfaces, Cottage cottage)
        {
            var soffitRidgeMetres = cottage.RidgeMetres - CottageForm.SoffitBelowTheSlatesMetres;
            foreach (var side in Sides)
            {
                var slates = new RoofSlope(cottage, side, cottage.RidgeMetres);
                var soffit = new RoofSlope(cottage, side, soffitRidgeMetres);
                surfaces.Add(Surface.Roof, slates.FacingUp());
                surfaces.Add(Surface.Timber, soffit.FacingDown());
            }
        }
    }
}
