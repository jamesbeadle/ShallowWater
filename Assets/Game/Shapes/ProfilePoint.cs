namespace ShallowWater.Game.Shapes
{
    public readonly struct ProfilePoint
    {
        public ProfilePoint(double radiusMetres, double heightMetres)
        {
            RadiusMetres = radiusMetres;
            HeightMetres = heightMetres;
        }

        public double RadiusMetres { get; }
        public double HeightMetres { get; }
    }
}
