using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class AskewShapes
    {
        private static readonly Dictionary<FigurePart, Func<SurfaceShapes>> Makers = new Dictionary<FigurePart, Func<SurfaceShapes>>
        {
            { FigurePart.Body, AskewBody.Torso },
            { FigurePart.Head, AskewBody.HeadAndCap },
            { FigurePart.LeftThigh, AskewLimbs.Thigh },
            { FigurePart.RightThigh, AskewLimbs.Thigh },
            { FigurePart.LeftShin, AskewLimbs.Shin },
            { FigurePart.RightShin, AskewLimbs.Shin },
            { FigurePart.LeftUpperArm, AskewLimbs.UpperArm },
            { FigurePart.RightUpperArm, AskewLimbs.UpperArm },
            { FigurePart.LeftForearm, AskewLimbs.Forearm },
            { FigurePart.RightForearm, AskewLimbs.Forearm }
        };

        public static SurfaceShapes Of(FigurePart part)
        {
            return Makers[part]();
        }
    }
}
