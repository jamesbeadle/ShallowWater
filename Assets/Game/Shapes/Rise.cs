namespace ShallowWater.Game.Shapes
{
    public readonly struct Rise
    {
        public Rise(double footMetres, double topMetres)
        {
            FootMetres = footMetres;
            TopMetres = topMetres;
        }

        public double FootMetres { get; }
        public double TopMetres { get; }
        public double HeightMetres => TopMetres - FootMetres;
    }
}
