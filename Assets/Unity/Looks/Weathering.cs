namespace ShallowWater.Unity.Looks
{
    public readonly struct Weathering
    {
        private const float Matt = 0;

        public static readonly Weathering Turf = new Weathering(7f, 0.18f, 0.35f, 0.08f, 0.03f);
        public static readonly Weathering Mud = new Weathering(3f, 0.12f, 0.3f, 0.2f, 0.02f);
        public static readonly Weathering Dirt = new Weathering(4f, 0.06f, 0.4f, 0.12f, 0.015f);
        public static readonly Weathering Tar = new Weathering(5f, 0.05f, 0.35f, 0.18f, 0.004f);
        public static readonly Weathering Gravel = new Weathering(3f, 0.04f, 0.5f, 0.1f, 0.015f);
        public static readonly Weathering Enamel = new Weathering(2f, 0.05f, 0.25f, 0.25f, 0.001f);
        public static readonly Weathering Timber = new Weathering(1f, 0.05f, 0.35f, 0.1f, 0.004f);
        public static readonly Weathering Bark = new Weathering(0.8f, 0.04f, 0.45f, 0.05f, 0.03f);
        public static readonly Weathering Terracotta = new Weathering(0.6f, 0.03f, 0.3f, 0.12f, 0.003f);
        public static readonly Weathering Tarpaulin = new Weathering(1.5f, 0.08f, 0.3f, 0.18f, 0.008f);
        public static readonly Weathering Tarred = new Weathering(2f, 0.06f, 0.2f, 0.35f, 0.003f);
        public static readonly Weathering Gloss = new Weathering(1.2f, 0.05f, 0.1f, 0.5f, 0.0015f);
        public static readonly Weathering CastIron = new Weathering(0.5f, 0.03f, 0.25f, 0.4f, 0.003f, 0.6f);
        public static readonly Weathering Tarnished = new Weathering(0.4f, 0.02f, 0.35f, 0.35f, 0.002f, 0.8f);
        public static readonly Weathering Glass = new Weathering(1f, 0.05f, 0.1f, 0.9f, 0.0005f);

        private Weathering(float patchMetres, float grainMetres, float grainAmount, float smoothness, float reliefMetres)
            : this(patchMetres, grainMetres, grainAmount, smoothness, reliefMetres, Matt)
        {
        }

        private Weathering(float patchMetres, float grainMetres, float grainAmount, float smoothness, float reliefMetres, float metallic)
        {
            PatchMetres = patchMetres;
            GrainMetres = grainMetres;
            GrainAmount = grainAmount;
            Smoothness = smoothness;
            ReliefMetres = reliefMetres;
            Metallic = metallic;
        }

        public float PatchMetres { get; }
        public float GrainMetres { get; }
        public float GrainAmount { get; }
        public float Smoothness { get; }
        public float ReliefMetres { get; }
        public float Metallic { get; }
    }
}
