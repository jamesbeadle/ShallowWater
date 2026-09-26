namespace ShallowWater.Game.Roads
{
    public readonly struct HedgeShape
    {
        public HedgeShape(double faceMetres, double topMetres, double topHeightMetres, double roughnessMetres)
        {
            FaceMetres = faceMetres;
            TopMetres = topMetres;
            TopHeightMetres = topHeightMetres;
            RoughnessMetres = roughnessMetres;
        }

        public double FaceMetres { get; }
        public double TopMetres { get; }
        public double TopHeightMetres { get; }
        public double RoughnessMetres { get; }
    }
}
