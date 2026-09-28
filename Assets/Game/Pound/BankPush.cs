using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Pound
{
    public readonly struct BankPush
    {
        public BankPush(GroundPoint offset, GroundPoint correction)
        {
            Offset = offset;
            Correction = correction;
        }

        public GroundPoint Offset { get; }
        public GroundPoint Correction { get; }
        public GroundPoint Away => Correction.Normalised;
        public double Depth => Correction.Length;
    }
}
