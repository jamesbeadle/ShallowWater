using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public sealed class PoseBlender
    {
        private const double SettleSeconds = 0.35;
        private const double SuddenRadiansAtLeast = 0.2;
        private const double SuddenRadiansPerSecond = 10;
        private const double SuddenMetres = 0.06;
        private const double Whole = 1;

        private Blending committed;
        private Blending pending;
        private double pendingAt = double.NaN;

        public FigurePose Towards(FigurePose target, double now)
        {
            var isANewFrame = now != pendingAt;
            if (isANewFrame) committed = pending;
            pendingAt = now;
            pending = Stepped(target, now);
            return pending.Shown;
        }

        private Blending Stepped(FigurePose target, double now)
        {
            var isTheFirst = committed == null;
            if (isTheFirst) return Blending.Fresh(target, now);
            var isSudden = IsSudden(target, now);
            var offset = isSudden ? FigurePose.Mixed((committed.Shown, Whole), (target, -Whole)) : committed.Offset;
            var heldSince = isSudden ? now : committed.HeldSince;
            var shown = FigurePose.Mixed((target, Whole), (offset, Remaining(now - heldSince)));
            return new Blending(shown, target, offset, heldSince, now);
        }

        private bool IsSudden(FigurePose target, double now)
        {
            var seconds = now - committed.ShownAt;
            var suddenRadians = Math.Max(SuddenRadiansAtLeast, SuddenRadiansPerSecond * seconds);
            var wanted = committed.Target;
            var isAJumpInAngle = wanted.LargestAngleDifferenceFrom(target) > suddenRadians;
            var moved = wanted.HipsOffset - target.HipsOffset;
            var isAJumpInPlace = moved.Length > SuddenMetres;
            return isAJumpInAngle || isAJumpInPlace;
        }

        private static double Remaining(double secondsHeld)
        {
            var settled = Math.Clamp(secondsHeld / SettleSeconds, 0, Whole);
            return Whole - Smoothing.Step(settled);
        }
    }
}
