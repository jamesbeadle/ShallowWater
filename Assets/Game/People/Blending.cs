namespace ShallowWater.Game.People
{
    public sealed class Blending
    {
        public Blending(FigurePose shown, FigurePose offset, double heldSince, double shownAt)
        {
            Shown = shown;
            Offset = offset;
            HeldSince = heldSince;
            ShownAt = shownAt;
        }

        public FigurePose Shown { get; }
        public FigurePose Offset { get; }
        public double HeldSince { get; }
        public double ShownAt { get; }

        public static Blending Fresh(FigurePose shown, double now)
        {
            return new Blending(shown, FigurePose.Upright, now, now);
        }
    }
}
