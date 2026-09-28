using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureJoints
    {
        private const double Level = 0;
        private const double Left = -1;
        private const double Right = 1;
        private const double ShoulderAboveTheHips = AskewForm.ShoulderMetres - AskewForm.HipMetres;

        public static readonly IReadOnlyDictionary<FigurePart, FigurePart> Parents = new Dictionary<FigurePart, FigurePart>
        {
            { FigurePart.Head, FigurePart.Body },
            { FigurePart.LeftThigh, FigurePart.Body }, { FigurePart.RightThigh, FigurePart.Body },
            { FigurePart.LeftShin, FigurePart.LeftThigh }, { FigurePart.RightShin, FigurePart.RightThigh },
            { FigurePart.LeftUpperArm, FigurePart.Body }, { FigurePart.RightUpperArm, FigurePart.Body },
            { FigurePart.LeftForearm, FigurePart.LeftUpperArm }, { FigurePart.RightForearm, FigurePart.RightUpperArm }
        };

        public static readonly IReadOnlyDictionary<FigurePart, Offset> FromTheParent = new Dictionary<FigurePart, Offset>
        {
            { FigurePart.Body, new Offset(Level, AskewForm.HipMetres, Level) },
            { FigurePart.Head, new Offset(Level, AskewForm.NeckMetres - AskewForm.HipMetres, Level) },
            { FigurePart.LeftThigh, new Offset(Left * AskewForm.HipHalfWidthMetres, Level, Level) },
            { FigurePart.RightThigh, new Offset(Right * AskewForm.HipHalfWidthMetres, Level, Level) },
            { FigurePart.LeftShin, new Offset(Level, -AskewForm.ThighMetres, Level) },
            { FigurePart.RightShin, new Offset(Level, -AskewForm.ThighMetres, Level) },
            { FigurePart.LeftUpperArm, new Offset(Left * AskewForm.ShoulderHalfWidthMetres, ShoulderAboveTheHips, Level) },
            { FigurePart.RightUpperArm, new Offset(Right * AskewForm.ShoulderHalfWidthMetres, ShoulderAboveTheHips, Level) },
            { FigurePart.LeftForearm, new Offset(Level, -AskewForm.UpperArmMetres, Level) },
            { FigurePart.RightForearm, new Offset(Level, -AskewForm.UpperArmMetres, Level) }
        };
    }
}
