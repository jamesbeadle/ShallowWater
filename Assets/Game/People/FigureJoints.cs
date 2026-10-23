using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureJoints
    {
        private const double Level = 0;
        private const double Left = -1;
        private const double Right = 1;

        public static readonly IReadOnlyDictionary<FigurePart, FigurePart> Parents = new Dictionary<FigurePart, FigurePart>
        {
            { FigurePart.Chest, FigurePart.Hips }, { FigurePart.Head, FigurePart.Chest },
            { FigurePart.LeftSkirt, FigurePart.Hips }, { FigurePart.RightSkirt, FigurePart.Hips },
            { FigurePart.LeftThigh, FigurePart.Hips }, { FigurePart.RightThigh, FigurePart.Hips },
            { FigurePart.LeftShin, FigurePart.LeftThigh }, { FigurePart.RightShin, FigurePart.RightThigh },
            { FigurePart.LeftFoot, FigurePart.LeftShin }, { FigurePart.RightFoot, FigurePart.RightShin },
            { FigurePart.LeftUpperArm, FigurePart.Chest }, { FigurePart.RightUpperArm, FigurePart.Chest },
            { FigurePart.LeftForearm, FigurePart.LeftUpperArm }, { FigurePart.RightForearm, FigurePart.RightUpperArm },
            { FigurePart.LeftHand, FigurePart.LeftForearm }, { FigurePart.RightHand, FigurePart.RightForearm }
        };

        public static readonly IReadOnlyDictionary<FigurePart, Offset> FromTheParent = new Dictionary<FigurePart, Offset>
        {
            { FigurePart.Hips, new Offset(Level, AskewForm.HipMetres, Level) },
            { FigurePart.Chest, new Offset(Level, AskewForm.WaistAboveTheHipsMetres, Level) },
            { FigurePart.Head, new Offset(Level, AskewForm.NeckAboveTheWaistMetres, Level) },
            { FigurePart.LeftSkirt, new Offset(Level, AskewForm.WaistAboveTheHipsMetres, Level) },
            { FigurePart.RightSkirt, new Offset(Level, AskewForm.WaistAboveTheHipsMetres, Level) },
            { FigurePart.LeftThigh, new Offset(Left * AskewForm.HipHalfWidthMetres, Level, Level) },
            { FigurePart.RightThigh, new Offset(Right * AskewForm.HipHalfWidthMetres, Level, Level) },
            { FigurePart.LeftShin, new Offset(Level, -AskewForm.ThighMetres, Level) },
            { FigurePart.RightShin, new Offset(Level, -AskewForm.ThighMetres, Level) },
            { FigurePart.LeftFoot, new Offset(Level, -AskewForm.ShinMetres, Level) },
            { FigurePart.RightFoot, new Offset(Level, -AskewForm.ShinMetres, Level) },
            { FigurePart.LeftUpperArm, new Offset(Left * AskewForm.ShoulderHalfWidthMetres, AskewForm.ShoulderAboveTheWaistMetres, Level) },
            { FigurePart.RightUpperArm, new Offset(Right * AskewForm.ShoulderHalfWidthMetres, AskewForm.ShoulderAboveTheWaistMetres, Level) },
            { FigurePart.LeftForearm, new Offset(Level, -AskewForm.UpperArmMetres, Level) },
            { FigurePart.RightForearm, new Offset(Level, -AskewForm.UpperArmMetres, Level) },
            { FigurePart.LeftHand, new Offset(Level, -AskewForm.ForearmMetres, Level) },
            { FigurePart.RightHand, new Offset(Level, -AskewForm.ForearmMetres, Level) }
        };

        public static readonly IReadOnlyDictionary<FigureSide, FigurePart> Hands = new Dictionary<FigureSide, FigurePart>
        {
            { FigureSide.Left, FigurePart.LeftHand }, { FigureSide.Right, FigurePart.RightHand }
        };

        public static readonly IReadOnlyDictionary<FigureSide, FigurePart> Feet = new Dictionary<FigureSide, FigurePart>
        {
            { FigureSide.Left, FigurePart.LeftFoot }, { FigureSide.Right, FigurePart.RightFoot }
        };

        public static readonly IReadOnlyDictionary<FigurePart, FigurePart> Opposites = new Dictionary<FigurePart, FigurePart>
        {
            { FigurePart.LeftSkirt, FigurePart.RightSkirt }, { FigurePart.RightSkirt, FigurePart.LeftSkirt },
            { FigurePart.LeftThigh, FigurePart.RightThigh }, { FigurePart.RightThigh, FigurePart.LeftThigh },
            { FigurePart.LeftShin, FigurePart.RightShin }, { FigurePart.RightShin, FigurePart.LeftShin },
            { FigurePart.LeftFoot, FigurePart.RightFoot }, { FigurePart.RightFoot, FigurePart.LeftFoot },
            { FigurePart.LeftUpperArm, FigurePart.RightUpperArm }, { FigurePart.RightUpperArm, FigurePart.LeftUpperArm },
            { FigurePart.LeftForearm, FigurePart.RightForearm }, { FigurePart.RightForearm, FigurePart.LeftForearm },
            { FigurePart.LeftHand, FigurePart.RightHand }, { FigurePart.RightHand, FigurePart.LeftHand }
        };
    }
}
