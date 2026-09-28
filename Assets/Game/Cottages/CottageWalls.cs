using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Cottages
{
    public static class CottageWalls
    {
        private const double Behind = -1;
        private const double InFront = 1;
        private const double AtTheMiddle = 0;
        private const double Half = 0.5;

        public static void Raise(SurfaceShapes surfaces, Cottage cottage)
        {
            var length = cottage.HalfLengthMetres;
            var width = cottage.HalfWidthMetres;
            var backLeft = cottage.At(-length, Behind * width);
            var frontLeft = cottage.At(length, Behind * width);
            var frontRight = cottage.At(length, InFront * width);
            var backRight = cottage.At(-length, InFront * width);
            LongWall(surfaces, cottage, backLeft, frontLeft);
            Gable(surfaces, cottage, frontLeft, frontRight);
            LongWall(surfaces, cottage, frontRight, backRight);
            Gable(surfaces, cottage, backRight, backLeft);
        }

        private static void LongWall(SurfaceShapes surfaces, Cottage cottage, GroundPoint from, GroundPoint to)
        {
            var wall = new Shape();
            Extrusion.AddWall(wall, from, to, -cottage.HalfLengthMetres, StoreysOf(cottage));
            surfaces.Add(Surface.Wall, wall);
        }

        private static void Gable(SurfaceShapes surfaces, Cottage cottage, GroundPoint from, GroundPoint to)
        {
            var gable = new Shape();
            var storeys = StoreysOf(cottage);
            var halfWidth = cottage.HalfWidthMetres;
            Extrusion.AddWall(gable, from, to, -halfWidth, storeys);
            var peak = (from + to) * Half;
            var fromEaves = gable.Add(new WorldPoint(from.East, storeys.TopMetres, from.North), new SurfacePlace(-halfWidth, storeys.HeightMetres));
            var toEaves = gable.Add(new WorldPoint(to.East, storeys.TopMetres, to.North), new SurfacePlace(halfWidth, storeys.HeightMetres));
            var ridgeHeight = cottage.RidgeMetres - storeys.FootMetres;
            var ridge = gable.Add(new WorldPoint(peak.East, cottage.RidgeMetres, peak.North), new SurfacePlace(AtTheMiddle, ridgeHeight));
            gable.AddTriangle(fromEaves, toEaves, ridge);
            surfaces.Add(Surface.GableWall, gable);
        }

        private static Rise StoreysOf(Cottage cottage)
        {
            return new Rise(CottageForm.FootMetres, cottage.EavesMetres);
        }
    }
}
