using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class ArmReach
    {
        private const double StraightArm = 1;
        private const double FoldedArm = -1;
        private const double Doubled = 2;

        public static ArmAngles Towards(Offset handFromTheShoulder, double upperArmMetres, double forearmMetres)
        {
            var reach = Math.Atan2(handFromTheShoulder.Eastward, -handFromTheShoulder.Upward);
            var down = Math.Sqrt(handFromTheShoulder.Eastward * handFromTheShoulder.Eastward + handFromTheShoulder.Upward * handFromTheShoulder.Upward);
            var forward = handFromTheShoulder.Northward;
            var distance = Math.Sqrt(down * down + forward * forward);
            var elbow = -ElbowFor(distance, upperArmMetres, forearmMetres);
            var toTheHand = Math.Atan2(forward, down);
            var upperArmOffTheLine = Math.Atan2(forearmMetres * Math.Sin(elbow), upperArmMetres + forearmMetres * Math.Cos(elbow));
            return new ArmAngles(toTheHand - upperArmOffTheLine, reach, elbow);
        }

        private static double ElbowFor(double distance, double upperArmMetres, double forearmMetres)
        {
            var squaresApart = distance * distance - upperArmMetres * upperArmMetres - forearmMetres * forearmMetres;
            var cosine = Math.Clamp(squaresApart / (Doubled * upperArmMetres * forearmMetres), FoldedArm, StraightArm);
            return Math.Acos(cosine);
        }
    }
}
