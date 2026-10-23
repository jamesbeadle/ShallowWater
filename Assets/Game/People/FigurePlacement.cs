using System;
using System.Collections.Generic;

namespace ShallowWater.Game.People
{
    public static class FigurePlacement
    {
        public static IReadOnlyDictionary<FigurePart, Frame> Of(FigurePose pose)
        {
            var frames = new Dictionary<FigurePart, Frame>();
            foreach (FigurePart part in Enum.GetValues(typeof(FigurePart))) frames[part] = FrameOf(part, pose, frames);
            return frames;
        }

        public static Frame ParentFrameOf(FigurePart part, IReadOnlyDictionary<FigurePart, Frame> frames)
        {
            var hasParent = FigureJoints.Parents.TryGetValue(part, out var parent);
            return hasParent ? frames[parent] : Frame.AtTheFeet;
        }

        private static Frame FrameOf(FigurePart part, FigurePose pose, IReadOnlyDictionary<FigurePart, Frame> frames)
        {
            var signed = JointAxes.Signed(part, pose.Of(part));
            var isTheRoot = !FigureJoints.Parents.ContainsKey(part);
            var rest = FigureJoints.FromTheParent[part];
            var offset = isTheRoot ? rest + pose.HipsOffset : rest;
            return ParentFrameOf(part, frames).Child(offset, signed);
        }
    }
}
