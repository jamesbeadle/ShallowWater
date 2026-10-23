using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class LimbReach
    {
        private const double ElbowFoldsForward = 1;
        private const double KneeFoldsBack = -1;
        private const double Doubled = 2;
        private const double ShortOfStraightMetres = 0.001;
        private const double HardlyAcross = 0.000001;
        private const double Straight = 1;
        private const double Folded = -1;
        private const double Opposite = -1;
        private static readonly WorldPoint AtTheJoint = new WorldPoint(0, 0, 0);

        public static LimbAngles Arm(Offset palmFromTheShoulder, double upperArmMetres, double handReachMetres, Offset elbowTowards)
        {
            return Towards(palmFromTheShoulder, upperArmMetres, handReachMetres, elbowTowards, ElbowFoldsForward);
        }

        public static LimbAngles Leg(Offset ankleFromTheHip, double thighMetres, double shinMetres, Offset kneeTowards)
        {
            return Towards(ankleFromTheHip, thighMetres, shinMetres, kneeTowards, KneeFoldsBack);
        }

        private static LimbAngles Towards(Offset endFromTheJoint, double upperMetres, double lowerMetres, Offset bendTowards, double foldWay)
        {
            var distance = Math.Min(endFromTheJoint.Length, upperMetres + lowerMetres - ShortOfStraightMetres);
            var axis = endFromTheJoint.Normalised;
            var sideways = RightAngleTo(axis, bendTowards);
            var cosine = (distance * distance + upperMetres * upperMetres - lowerMetres * lowerMetres) / (Doubled * distance * upperMetres);
            var offTheAxis = Math.Acos(Math.Clamp(cosine, Folded, Straight));
            var middle = axis * (upperMetres * Math.Cos(offTheAxis)) + sideways * (upperMetres * Math.Sin(offTheAxis));
            var upper = middle.Normalised;
            var lower = (axis * distance - middle).Normalised;
            var fold = Math.Acos(Math.Clamp(upper.Dot(lower), Folded, Straight));
            var forward = FoldSide(upper, lower, sideways) * foldWay;
            var up = upper * Opposite;
            var wanted = new Frame(AtTheJoint, up.Cross(forward), up, forward);
            return new LimbAngles(Frame.AtTheFeet.SignedAnglesOf(wanted), fold * foldWay);
        }

        private static Offset RightAngleTo(Offset axis, Offset towards)
        {
            var across = towards - axis * towards.Dot(axis);
            var isAlongTheAxis = across.Length < HardlyAcross;
            return isAlongTheAxis ? axis.AnyRightAngle() : across.Normalised;
        }

        private static Offset FoldSide(Offset upper, Offset lower, Offset sideways)
        {
            var across = lower - upper * lower.Dot(upper);
            var isStraight = across.Length < HardlyAcross;
            return isStraight ? sideways * Opposite : across.Normalised;
        }
    }
}
