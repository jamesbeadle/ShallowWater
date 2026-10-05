namespace ShallowWater.Unity.Looks
{
    public readonly struct RoofCourses
    {
        public static readonly RoofCourses Slates = new RoofCourses(0.3f, 0.22f, 0.006f, 0.3f, 0.5f);
        public static readonly RoofCourses PlainTiles = new RoofCourses(0.165f, 0.1f, 0.009f, 0.18f, 0.8f);

        private RoofCourses(float unitWidthMetres, float gaugeMetres, float lapMetres, float smoothness, float moss)
        {
            UnitWidthMetres = unitWidthMetres;
            GaugeMetres = gaugeMetres;
            LapMetres = lapMetres;
            Smoothness = smoothness;
            Moss = moss;
        }

        public float UnitWidthMetres { get; }
        public float GaugeMetres { get; }
        public float LapMetres { get; }
        public float Smoothness { get; }
        public float Moss { get; }
    }
}
