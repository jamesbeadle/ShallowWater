namespace ShallowWater.Unity.Looks
{
    public readonly struct CropPattern
    {
        public CropPattern(CropPatternKind kind, float rowMetres, float cover, float reliefMetres, float lifted)
        {
            Kind = kind;
            RowMetres = rowMetres;
            Cover = cover;
            ReliefMetres = reliefMetres;
            Lifted = lifted;
        }

        public CropPatternKind Kind { get; }
        public float RowMetres { get; }
        public float Cover { get; }
        public float ReliefMetres { get; }
        public float Lifted { get; }
    }
}
