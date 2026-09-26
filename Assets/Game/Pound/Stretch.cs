namespace ShallowWater.Game.Pound
{
    public readonly struct Stretch
    {
        public Stretch(Span span, bool isVillage, bool isMooring)
        {
            Span = span;
            IsVillage = isVillage;
            IsMooring = isMooring;
        }

        public Span Span { get; }
        public bool IsVillage { get; }
        public bool IsMooring { get; }
        public bool HasHardEdge => IsVillage || IsMooring;
    }
}
