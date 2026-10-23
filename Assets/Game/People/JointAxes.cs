using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class JointAxes
    {
        private const double Same = 1;
        private const double Opposite = -1;

        public static readonly IReadOnlyDictionary<FigurePart, FigureSide> Sides = new Dictionary<FigurePart, FigureSide>
        {
            { FigurePart.Hips, FigureSide.Centre }, { FigurePart.Chest, FigureSide.Centre }, { FigurePart.Head, FigureSide.Centre },
            { FigurePart.LeftSkirt, FigureSide.Left }, { FigurePart.RightSkirt, FigureSide.Right },
            { FigurePart.LeftThigh, FigureSide.Left }, { FigurePart.LeftShin, FigureSide.Left }, { FigurePart.LeftFoot, FigureSide.Left },
            { FigurePart.RightThigh, FigureSide.Right }, { FigurePart.RightShin, FigureSide.Right }, { FigurePart.RightFoot, FigureSide.Right },
            { FigurePart.LeftUpperArm, FigureSide.Left }, { FigurePart.LeftForearm, FigureSide.Left }, { FigurePart.LeftHand, FigureSide.Left },
            { FigurePart.RightUpperArm, FigureSide.Right }, { FigurePart.RightForearm, FigureSide.Right }, { FigurePart.RightHand, FigureSide.Right }
        };

        public static readonly IReadOnlyDictionary<FigurePart, RestDirection> Rests = new Dictionary<FigurePart, RestDirection>
        {
            { FigurePart.Hips, RestDirection.Up }, { FigurePart.Chest, RestDirection.Up }, { FigurePart.Head, RestDirection.Up },
            { FigurePart.LeftSkirt, RestDirection.Down }, { FigurePart.RightSkirt, RestDirection.Down },
            { FigurePart.LeftThigh, RestDirection.Down }, { FigurePart.LeftShin, RestDirection.Down }, { FigurePart.LeftFoot, RestDirection.Forward },
            { FigurePart.RightThigh, RestDirection.Down }, { FigurePart.RightShin, RestDirection.Down }, { FigurePart.RightFoot, RestDirection.Forward },
            { FigurePart.LeftUpperArm, RestDirection.Down }, { FigurePart.LeftForearm, RestDirection.Down }, { FigurePart.LeftHand, RestDirection.Down },
            { FigurePart.RightUpperArm, RestDirection.Down }, { FigurePart.RightForearm, RestDirection.Down }, { FigurePart.RightHand, RestDirection.Down }
        };

        public static JointAngles Signed(FigurePart part, JointAngles angles)
        {
            var swing = angles.SwingRadians * SwingSign(part);
            var reach = angles.ReachRadians * ReachSign(part);
            var twist = angles.TwistRadians * TwistSign(part);
            return new JointAngles(swing, reach, twist);
        }

        public static Offset Outward(FigurePart part, Offset fromTheJoint)
        {
            var isMirrored = Sides[part] == FigureSide.Left;
            if (!isMirrored) return fromTheJoint;
            return new Offset(-fromTheJoint.Eastward, fromTheJoint.Upward, fromTheJoint.Northward);
        }

        private static double SwingSign(FigurePart part)
        {
            var isHanging = Rests[part] == RestDirection.Down;
            return isHanging ? Opposite : Same;
        }

        private static double ReachSign(FigurePart part)
        {
            var isOnTheRight = Sides[part] == FigureSide.Right;
            return isOnTheRight ? Same : Opposite;
        }

        private static double TwistSign(FigurePart part)
        {
            var isOnTheLeft = Sides[part] == FigureSide.Left;
            return isOnTheLeft ? Opposite : Same;
        }
    }
}
