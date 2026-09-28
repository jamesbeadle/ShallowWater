namespace ShallowWater.Unity.Looks
{
    public readonly struct Weathering
    {
        private const float Matt = 0;

        public static readonly Weathering Turf = new Weathering(7f, 0.18f, 0.35f, 0.08f);
        public static readonly Weathering Mud = new Weathering(3f, 0.12f, 0.3f, 0.2f);
        public static readonly Weathering Dirt = new Weathering(4f, 0.06f, 0.4f, 0.12f);
        public static readonly Weathering Tar = new Weathering(5f, 0.05f, 0.35f, 0.18f);
        public static readonly Weathering Gravel = new Weathering(3f, 0.04f, 0.5f, 0.1f);
        public static readonly Weathering Enamel = new Weathering(2f, 0.05f, 0.25f, 0.25f);
        public static readonly Weathering Timber = new Weathering(1f, 0.05f, 0.35f, 0.1f);
        public static readonly Weathering Bark = new Weathering(0.8f, 0.04f, 0.45f, 0.05f);
        public static readonly Weathering Terracotta = new Weathering(0.6f, 0.03f, 0.3f, 0.12f);
        public static readonly Weathering Tarpaulin = new Weathering(1.5f, 0.08f, 0.3f, 0.18f);
        public static readonly Weathering Tarred = new Weathering(2f, 0.06f, 0.2f, 0.35f);
        public static readonly Weathering Gloss = new Weathering(1.2f, 0.05f, 0.1f, 0.5f);
        public static readonly Weathering CastIron = new Weathering(0.5f, 0.03f, 0.25f, 0.4f, 0.6f);
        public static readonly Weathering Polished = new Weathering(0.4f, 0.02f, 0.15f, 0.75f, 1f);

        private Weathering(float patchMetres, float grainMetres, float grainAmount, float smoothness)
            : this(patchMetres, grainMetres, grainAmount, smoothness, Matt)
        {
        }

        private Weathering(float patchMetres, float grainMetres, float grainAmount, float smoothness, float metallic)
        {
            PatchMetres = patchMetres;
            GrainMetres = grainMetres;
            GrainAmount = grainAmount;
            Smoothness = smoothness;
            Metallic = metallic;
        }

        public float PatchMetres { get; }
        public float GrainMetres { get; }
        public float GrainAmount { get; }
        public float Smoothness { get; }
        public float Metallic { get; }
    }
}
