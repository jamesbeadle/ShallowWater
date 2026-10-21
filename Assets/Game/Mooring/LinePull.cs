using System;
using ShallowWater.Game.Boat;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Mooring
{
    public static class LinePull
    {
        public const double GiveMetres = 0.6;
        private const double HardestPullNewtons = 3000;
        private const double NewtonsPerMetreOfStretch = HardestPullNewtons / GiveMetres;
        private const double NewtonsPerMetrePerSecondOfStretching = 9000;
        private const double Slack = 0;
        private const double NoReach = 1e-6;

        public static BoatForces On(BoatMotion boat, HullPoint fixedEnd, GroundPoint anchor, GroundPoint anchorVelocity, double lengthMetres)
        {
            var end = boat.PointOf(fixedEnd);
            var toTheAnchor = anchor - end;
            var isAtTheAnchor = toTheAnchor.Length < NoReach;
            if (isAtTheAnchor) return BoatForces.None;
            var stretch = toTheAnchor.Length - lengthMetres;
            if (stretch <= Slack) return BoatForces.None;
            var towardsTheAnchor = toTheAnchor.Normalised;
            var stretching = (anchorVelocity - EndVelocity(boat, end - boat.Position)).Dot(towardsTheAnchor);
            var pull = stretch * NewtonsPerMetreOfStretch + stretching * NewtonsPerMetrePerSecondOfStretching;
            return Along(boat, fixedEnd, towardsTheAnchor, Math.Clamp(pull, Slack, HardestPullNewtons));
        }

        private static GroundPoint EndVelocity(BoatMotion boat, GroundPoint offset)
        {
            var spin = new GroundPoint(offset.North, -offset.East) * boat.SwingRadiansPerSecond;
            return boat.Velocity + spin;
        }

        private static BoatForces Along(BoatMotion boat, HullPoint fixedEnd, GroundPoint direction, double pull)
        {
            var ahead = direction.Dot(boat.Heading) * pull;
            var toStarboard = direction.Dot(boat.Starboard) * pull;
            var swinging = fixedEnd.Ahead * toStarboard - fixedEnd.ToStarboard * ahead;
            return new BoatForces(ahead, toStarboard, swinging);
        }
    }
}
