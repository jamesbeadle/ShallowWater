using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public readonly struct Stretch
    {
        public Stretch(Span span, bool isVillage, bool isMooring, bool isAtAnArmMouth)
        {
            Span = span;
            IsVillage = isVillage;
            IsMooring = isMooring;
            IsAtAnArmMouth = isAtAnArmMouth;
        }

        public Span Span { get; }
        public bool IsVillage { get; }
        public bool IsMooring { get; }
        public bool IsAtAnArmMouth { get; }
        public bool HasHardEdge => IsVillage || IsMooring;
    }
}
