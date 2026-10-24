namespace ShallowWater.Game.People
{
    public sealed class Blending
    {
        public Blending(FigurePose shown, FigurePose target, FigurePose offset, double heldSince, double shownAt)
        {
            Shown = shown;
            Target = target;
            Offset = offset;
            HeldSince = heldSince;
            ShownAt = shownAt;
        }

        public FigurePose Shown { get; }
        public FigurePose Target { get; }
        public FigurePose Offset { get; }
        public double HeldSince { get; }
        public double ShownAt { get; }

        public static Blending Fresh(FigurePose target, double now)
        {
            return new Blending(target, target, FigurePose.Upright, now, now);
        }
    }
}
