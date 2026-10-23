using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class Turns
    {
        private const double OnAStud = 2;
        private const double OnAPost = 2;
        private const double FullTurnRadians = 2 * Math.PI;

        public static List<WorldPoint> UpTheStud(LineEnd stud, GroundPoint leadingTo)
        {
            var leaving = (leadingTo - stud.Place).Bearing;
            var tie = stud.Tie;
            var foot = new WorldPoint(tie.East, tie.Height - OnAStud * Rope.DiameterMetres, tie.North);
            return Coil.Rising(foot, stud.RadiusMetres + Rope.RadiusMetres, leaving - OnAStud * FullTurnRadians, OnAStud, Rope.DiameterMetres);
        }

        public static List<WorldPoint> DownThePost(MooringPost post, GroundPoint leadingFrom)
        {
            var arriving = (leadingFrom - post.Place).Bearing;
            return Coil.Rising(post.Tie, post.RadiusMetres + Rope.RadiusMetres, arriving, OnAPost, -Rope.DiameterMetres);
        }
    }
}
