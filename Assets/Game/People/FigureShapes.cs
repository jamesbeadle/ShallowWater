using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class FigureShapes
    {
        private static readonly Dictionary<FigurePart, Func<Outfit, SurfaceShapes>> Makers = new Dictionary<FigurePart, Func<Outfit, SurfaceShapes>>
        {
            { FigurePart.Hips, FigureHips.Seat },
            { FigurePart.Chest, AskewCoat.Dress },
            { FigurePart.Head, AskewHead.Shape },
            { FigurePart.LeftSkirt, outfit => AskewSkirt.Hung(FigureSide.Left, outfit) },
            { FigurePart.RightSkirt, outfit => AskewSkirt.Hung(FigureSide.Right, outfit) },
            { FigurePart.LeftThigh, FigureLimbs.Thigh },
            { FigurePart.RightThigh, FigureLimbs.Thigh },
            { FigurePart.LeftShin, FigureLimbs.Shin },
            { FigurePart.RightShin, FigureLimbs.Shin },
            { FigurePart.LeftFoot, outfit => Boot.Shaped() },
            { FigurePart.RightFoot, outfit => Boot.Shaped() },
            { FigurePart.LeftUpperArm, FigureLimbs.UpperArm },
            { FigurePart.RightUpperArm, FigureLimbs.UpperArm },
            { FigurePart.LeftForearm, FigureLimbs.Forearm },
            { FigurePart.RightForearm, FigureLimbs.Forearm },
            { FigurePart.LeftHand, outfit => Hand.Relaxed(FigureSide.Left) },
            { FigurePart.RightHand, outfit => Hand.Relaxed(FigureSide.Right) }
        };

        public static SurfaceShapes Of(FigurePart part, Outfit outfit)
        {
            return Makers[part](outfit);
        }
    }
}
