using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class LineInHand
    {
        public const double HandOutMetres = 0.33;
        private const double HandBelowTheShoulderMetres = 0.5;
        private const double Level = 0;
        private static readonly Offset ElbowBackAndDown = new Offset(0, -1, -0.6);

        public static FigurePart HandFor(Offset studFromTheFeet)
        {
            var isToTheRight = studFromTheFeet.Eastward >= Level;
            return isToTheRight ? FigurePart.RightHand : FigurePart.LeftHand;
        }

        public static FigurePose Holding(FigurePose stride, Offset studFromTheFeet)
        {
            var towardsTheStud = new Offset(studFromTheFeet.Eastward, Level, studFromTheFeet.Northward).Normalised;
            var isAtTheStud = double.IsNaN(towardsTheStud.Eastward);
            if (isAtTheStud) return stride;
            var hand = HandFor(studFromTheFeet);
            var forearm = FigureJoints.Parents[hand];
            var upperArm = FigureJoints.Parents[forearm];
            var frames = FigurePlacement.Of(stride);
            var shoulder = frames[upperArm];
            var palm = (towardsTheStud * HandOutMetres + Offset.Up * -HandBelowTheShoulderMetres).From(shoulder.Origin);
            return Arms.Reaching(stride, hand, palm, ElbowBackAndDown);
        }
    }
}
