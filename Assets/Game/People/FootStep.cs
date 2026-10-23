using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public readonly struct FootStep
    {
        public FootStep(WorldPoint ankle, double toesDownRadians, double toeOutRadians)
        {
            Ankle = ankle;
            ToesDownRadians = toesDownRadians;
            ToeOutRadians = toeOutRadians;
        }

        public WorldPoint Ankle { get; }
        public double ToesDownRadians { get; }
        public double ToeOutRadians { get; }

        public static FootStep Flat(double eastMetres, double northMetres, double toeOutRadians)
        {
            return new FootStep(new WorldPoint(eastMetres, AskewForm.AnkleAboveTheSoleMetres, northMetres), 0, toeOutRadians);
        }

        public FootStep Blend(FootStep towards, double share)
        {
            var moved = Offset.Between(Ankle, towards.Ankle) * share;
            var toesDown = ToesDownRadians + (towards.ToesDownRadians - ToesDownRadians) * share;
            var toeOut = ToeOutRadians + (towards.ToeOutRadians - ToeOutRadians) * share;
            return new FootStep(moved.From(Ankle), toesDown, toeOut);
        }
    }
}
