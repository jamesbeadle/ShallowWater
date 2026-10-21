using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Hand
    {
        private static readonly Offset PalmShape = new Offset(0.021, 0.08, 0.04);
        private static readonly Offset ThumbShape = new Offset(0.013, 0.038, 0.015);
        public const double PalmBelowTheWristMetres = 0.068;
        private static readonly Offset PalmBelowTheWrist = new Offset(0, -PalmBelowTheWristMetres, 0.012);
        private static readonly Offset ThumbBelowTheWrist = new Offset(0, -0.032, 0.055);

        public static Shape Hanging(WorldPoint wrist)
        {
            var palm = Ball.Shaped(PalmBelowTheWrist.From(wrist), PalmShape, AskewForm.FeatureSides);
            palm.Append(Ball.Shaped(ThumbBelowTheWrist.From(wrist), ThumbShape, AskewForm.FeatureSides));
            return palm;
        }
    }
}
