namespace ShallowWater.Unity.Looks
{
    public readonly struct BarkPattern
    {
        public BarkPattern(float kind, float furrowsAround, float furrowMetres, float reliefMetres, float lichens)
        {
            Kind = kind;
            FurrowsAround = furrowsAround;
            FurrowMetres = furrowMetres;
            ReliefMetres = reliefMetres;
            Lichens = lichens;
        }

        public float Kind { get; }
        public float FurrowsAround { get; }
        public float FurrowMetres { get; }
        public float ReliefMetres { get; }
        public float Lichens { get; }
    }
}
