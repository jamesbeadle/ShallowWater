using ShallowWater.Game.Boat;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class TillerHold
    {
        private static readonly Offset ElbowOutAndBack = new Offset(1, -0.2, -0.6);
        private static readonly WorldPoint AtTheFeet = new WorldPoint(0, 0, 0);

        public static FigurePose At(double rudder, double seconds)
        {
            var stance = HelmStance.At(rudder, seconds);
            var hand = HelmStance.HandFor(rudder);
            var forearm = FigureJoints.Parents[hand];
            var upperArm = FigureJoints.Parents[forearm];
            var frames = FigurePlacement.Of(stance);
            var shoulder = frames[upperArm];
            var grip = TillerGrip.FromTheFeet(Tiller.TurnedRadians(rudder), Offset.Between(AtTheFeet, shoulder.Origin));
            var holding = Arms.Reaching(stance, hand, grip.From(AtTheFeet), JointAxes.Outward(hand, ElbowOutAndBack));
            return Skirts.Following(holding);
        }
    }
}
