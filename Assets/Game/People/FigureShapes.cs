using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureShapes
    {
        private static readonly Dictionary<FigurePart, Func<Outfit, SurfaceShapes>> Makers = new Dictionary<FigurePart, Func<Outfit, SurfaceShapes>>
        {
            { FigurePart.Body, FigureBody.Torso },
            { FigurePart.Head, FigureBody.HeadAndCap },
            { FigurePart.LeftThigh, FigureLimbs.Thigh },
            { FigurePart.RightThigh, FigureLimbs.Thigh },
            { FigurePart.LeftShin, FigureLimbs.Shin },
            { FigurePart.RightShin, FigureLimbs.Shin },
            { FigurePart.LeftUpperArm, FigureLimbs.UpperArm },
            { FigurePart.RightUpperArm, FigureLimbs.UpperArm },
            { FigurePart.LeftForearm, FigureLimbs.Forearm },
            { FigurePart.RightForearm, FigureLimbs.Forearm }
        };

        public static SurfaceShapes Of(FigurePart part, Outfit outfit)
        {
            return Makers[part](outfit);
        }
    }
}
